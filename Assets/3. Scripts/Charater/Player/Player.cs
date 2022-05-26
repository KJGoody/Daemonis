using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class TargetGroup
{
    public string GroupName;
    public List<Transform> Targets = new List<Transform>();

    public TargetGroup(string groupName, Transform target)
    {
        GroupName = groupName;
        Targets.Add(target);
    }
}

public class Player : Character
{
    // 싱글톤
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    private FloatingJoystick joy;
    [SerializeField]
    private Transform exitPoint;    // 발사체 생성 위치
    [HideInInspector]
    public Vector2 atkDir;          // 넉백 방향 벡터

    [SerializeField]                // 발화와 특정 타겟들의 묶음
    private List<TargetGroup> targetGroups = new List<TargetGroup>();

    protected override void Start()
    {
        joy = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();

        base.Start();
    }
    protected override void Update()
    {
        GetInput();
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void GetInput()
    {
        Vector2 moveVector;         // 조이스틱에서 움직을 받기위한 벡터
        if (!IsAttacking)
        {
            moveVector.x = joy.Horizontal;
            moveVector.y = joy.Vertical;

            Direction = moveVector;
            if (moveVector.x != 0 && moveVector.y != 0)
                atkDir = moveVector;

            if (IsMoving)
            {
                Animator runParticle = GetComponent<Animator>();
                runParticle.SetTrigger("Run");
            }
        }
    }

    public void CastSpell(string spellIName)
    {
        if (MyTarget == null && SearchEnemy())
            AutoTarget();

        if (!IsAttacking)
            StartCoroutine(CastingSpell(spellIName));
    }

    private bool SearchEnemy() // 씬에 적이 존재하는지 검색
    {
        if (GameObject.FindWithTag("Enemy") == null)
            return false;
        else
            return true;
    }

    private void AutoTarget() // 가장 가까운적 타겟팅
    {
        if (FindNearestObject() != null)
            MyTarget = FindNearestObject().transform;
        else
            MyTarget = null;
    }

    private GameObject FindNearestObject()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 7, LayerMask.GetMask("HitBox"));

        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < collisions.Length; i++)
            if (collisions[i].CompareTag("Enemy"))       // 테그가 적인것을 찾는다.
                objects.Add(collisions[i].gameObject);

        if (objects.Count == 0)     // 만약 리스트가 비었다면 null을 반환
            return null;

        var neareastObject = objects        // 거리가 가장 짧은 오브젝트를 구한다.
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }

    private IEnumerator CastingSpell(string spellIName)
    {
        IsAttacking = true;
        _prefabs.PlayAnimation(4);                                  // 공격 애니메이션 재생
        if (MyTarget != null) LookAtTarget();                       // 타겟 바라보기

        Spell newSpell = SpellBook.MyInstance.GetSpell(spellIName); // 스펠북에서 스킬 받아옴
        if (newSpell.spellType.Equals(Spell.SpellType.Immediate))   // 점화 공격인지 확인   
        {
            for (int i = targetGroups.Count - 1; i >= 0; i--)       // 점화 공격 타겟 그룹확인
            {
                if (targetGroups[i].GroupName.Equals("Skill_Fire_02_Debuff"))
                    for (int j = targetGroups[i].Targets.Count - 1; j >= 0; j--)
                    {
                        if (Vector2.Distance(transform.position, targetGroups[i].Targets[j].position) < 7)      // 너무 멀리있는 타켓은 공격하지 않음
                        {
                            SpellScript spellScript = Instantiate(newSpell.MySpellPrefab, targetGroups[i].Targets[j]).GetComponent<SpellScript>();
                            spellScript.MyTarget = targetGroups[i].Targets[j];
                            // 발화 스택을 가지고 데미지 설정
                            spellScript.damage = targetGroups[i].Targets[j].transform.GetComponent<EnemyBase>().GetBuff("Skill_Fire_02_Debuff").BuffStack * 1;
                            targetGroups[i].Targets[j].transform.GetComponent<EnemyBase>().OffBuff("Skill_Fire_02_Debuff");
                        }
                    }
            }

            yield return new WaitForSeconds(0.3f);
            IsAttacking = false;
        }
        else
        {
            SpellScript spellScript = InstantiateSpell(newSpell);
            spellScript.atkDir = atkDir;
            if (MyTarget != null)
                spellScript.MyTarget = MyTarget;

            yield return new WaitForSeconds(spellScript.CastTime);
            IsAttacking = false;
        }
    }

    private SpellScript InstantiateSpell(Spell spell)       // 스팰 생성 함수
    {
        switch (spell.spellType)
        {
            case Spell.SpellType.Launch:
                return Instantiate(spell.MySpellPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();

            case Spell.SpellType.AOE:
                if (MyTarget != null)           // 타겟이 있을 때 정상 출력
                    return Instantiate(spell.MySpellPrefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
                else                            // 타겟이 없을 때 생성 위치 설정
                {
                    Vector3 ExitPoint = transform.position + new Vector3(atkDir.x, atkDir.y, 0).normalized * 2;
                    if (ExitPoint == transform.position)
                    {
                        if (_prefabs.transform.localScale.x == -1)
                            ExitPoint += new Vector3(1, 0, 0).normalized * 2;
                        else
                            ExitPoint += new Vector3(-1, 0, 0).normalized * 2;
                    }
                    return Instantiate(spell.MySpellPrefab, ExitPoint, Quaternion.identity).GetComponent<SpellScript>();
                }

            case Spell.SpellType.Toggle:
                return Instantiate(spell.MySpellPrefab, transform).GetComponent<SpellScript>();

            case Spell.SpellType.AE:
                if (MyTarget != null)
                    return Instantiate(spell.MySpellPrefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
                else
                {
                    Vector3 ExitPoint = transform.position + new Vector3(atkDir.x, atkDir.y, 0).normalized * 2;
                    if (ExitPoint == transform.position)
                    {
                        if (_prefabs.transform.localScale.x == -1)
                            ExitPoint += new Vector3(1, 0, 0).normalized * 2;
                        else
                            ExitPoint += new Vector3(-1, 0, 0).normalized * 2;
                    }
                    return Instantiate(spell.MySpellPrefab, ExitPoint, Quaternion.identity).GetComponent<SpellScript>();
                }
        }
        return null;
    }

    public void AddTarget(string TargetGroupName, Transform Target)
    {
        bool IsAlreadyIn = false;
        int Index = 0;

        if (targetGroups.Count > 0)
        {
            for (int i = 0; i < targetGroups.Count; i++)
                if (targetGroups[i].GroupName.Equals(TargetGroupName))
                {
                    IsAlreadyIn = true;
                    Index = i;
                }

            if (IsAlreadyIn)
            {
                if (!targetGroups[Index].Targets.Contains(Target.transform))    // 중복 확인
                    targetGroups[Index].Targets.Add(Target.transform);
            }
            else
                targetGroups.Add(new TargetGroup(TargetGroupName, Target.transform));
        }
        else
            targetGroups.Add(new TargetGroup(TargetGroupName, Target.transform));
    }

    public void RemoveTarget(string TargetGroupName, Transform Target)
    {
        for (int i = targetGroups.Count - 1; i >= 0; i--)
            if (targetGroups[i].GroupName.Equals(TargetGroupName))
                if (targetGroups[i].Targets.Contains(Target))
                {
                    targetGroups[i].Targets.Remove(Target);
                    if (targetGroups[i].Targets.Count == 0)
                        targetGroups.Remove(targetGroups[i]);
                }
    }


}
