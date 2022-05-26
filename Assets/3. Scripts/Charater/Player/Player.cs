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
    // �̱���
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
    private Transform exitPoint;    // �߻�ü ���� ��ġ
    [HideInInspector]
    public Vector2 atkDir;          // �˹� ���� ����

    [SerializeField]                // ��ȭ�� Ư�� Ÿ�ٵ��� ����
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
        Vector2 moveVector;         // ���̽�ƽ���� ������ �ޱ����� ����
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

    private bool SearchEnemy() // ���� ���� �����ϴ��� �˻�
    {
        if (GameObject.FindWithTag("Enemy") == null)
            return false;
        else
            return true;
    }

    private void AutoTarget() // ���� ������� Ÿ����
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
            if (collisions[i].CompareTag("Enemy"))       // �ױװ� ���ΰ��� ã�´�.
                objects.Add(collisions[i].gameObject);

        if (objects.Count == 0)     // ���� ����Ʈ�� ����ٸ� null�� ��ȯ
            return null;

        var neareastObject = objects        // �Ÿ��� ���� ª�� ������Ʈ�� ���Ѵ�.
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
        _prefabs.PlayAnimation(4);                                  // ���� �ִϸ��̼� ���
        if (MyTarget != null) LookAtTarget();                       // Ÿ�� �ٶ󺸱�

        Spell newSpell = SpellBook.MyInstance.GetSpell(spellIName); // ����Ͽ��� ��ų �޾ƿ�
        if (newSpell.spellType.Equals(Spell.SpellType.Immediate))   // ��ȭ �������� Ȯ��   
        {
            for (int i = targetGroups.Count - 1; i >= 0; i--)       // ��ȭ ���� Ÿ�� �׷�Ȯ��
            {
                if (targetGroups[i].GroupName.Equals("Skill_Fire_02_Debuff"))
                    for (int j = targetGroups[i].Targets.Count - 1; j >= 0; j--)
                    {
                        if (Vector2.Distance(transform.position, targetGroups[i].Targets[j].position) < 7)      // �ʹ� �ָ��ִ� Ÿ���� �������� ����
                        {
                            SpellScript spellScript = Instantiate(newSpell.MySpellPrefab, targetGroups[i].Targets[j]).GetComponent<SpellScript>();
                            spellScript.MyTarget = targetGroups[i].Targets[j];
                            // ��ȭ ������ ������ ������ ����
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

    private SpellScript InstantiateSpell(Spell spell)       // ���� ���� �Լ�
    {
        switch (spell.spellType)
        {
            case Spell.SpellType.Launch:
                return Instantiate(spell.MySpellPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();

            case Spell.SpellType.AOE:
                if (MyTarget != null)           // Ÿ���� ���� �� ���� ���
                    return Instantiate(spell.MySpellPrefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
                else                            // Ÿ���� ���� �� ���� ��ġ ����
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
                if (!targetGroups[Index].Targets.Contains(Target.transform))    // �ߺ� Ȯ��
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
