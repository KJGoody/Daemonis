using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : Character
{
        // 싱글톤
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    public EquipmentItem[] usingEquipment = new EquipmentItem[6];

    public delegate void UseEquipment(int partNum);
    public event UseEquipment useEquipment;

    private FloatingJoystick joy;

    [SerializeField]
    private Transform exitPoint; // 발사체 생성 위치

    public Vector2 atkDir;
    //public Transform myTarget { get; set; }
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
        ///THIS IS USED FOR DEBUGGING ONLY
        ///
        if (Input.GetKeyDown(KeyCode.I))
        {
            stat.CurrentHealth -= 10;
            stat.CurrentMana -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            stat.CurrentHealth += 10;
            stat.CurrentMana += 10;
        }

        Vector2 moveVector;
        if (!IsAttacking)
        {
            moveVector.x = joy.Horizontal;
            moveVector.y = joy.Vertical;
            Direction = moveVector;
            if(moveVector.x != 0 && moveVector.y != 0)
                atkDir = moveVector;
            if (IsMoving)
            {
                Animator runParticle = GetComponent<Animator>();
                runParticle.SetTrigger("Run");
            }
        }
     
         
    }
    public override void FindTarget()   //공격하는 타겟 방향 바라보기
    {
        atkDir = Direction;
        base.FindTarget();
    }
    private IEnumerator Attack(string spellIName)
    {
        Spell newSpell = SpellBook.MyInstance.CastSpell(spellIName); //스펠북에서 스킬 받아옴
        IsAttacking = true;
        _prefabs.PlayAnimation(4);
        if (MyTarget != null) // 밑에랑 나눠둘수밖에 없음
        {
            FindTarget();
        }

        GameObject spell = newSpell.MySpellPrefab;

        SpellScript s = Instantiate(spell, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initailize(newSpell.MyDamage,transform,atkDir);

        if (MyTarget != null) //위에랑 나눠둘수밖에 없음
        {
            s.MyTarget = MyTarget;
        }
        
        yield return new WaitForSeconds(0.3f); // 테스트를 위한 코드입니다. 여기다가 후딜넣을까 생각중
        StopAttack();
       
    }
    private void AutoTarget() // 가장 가까운적 타겟팅
    {
        MyTarget = FindNearestObjectByTag("HitBox").GetComponent<Transform>();
        if(Vector2.Distance(MyTarget.position,transform.position) > 7) // 너무 멀면 타겟 해제
        {
            MyTarget = null;
        }
    }
    private GameObject FindNearestObjectByTag(string tag)
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }
    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            IsAttacking = false;
        }
    }
    public bool SearchEnemy() // 씬에 적이 존재하는지 검색
    {
        if (GameObject.FindWithTag("HitBox") == null)
            return false;
        else
            return true;
    }
    public void CastSpell(string spellIName)
    {
        if (MyTarget == null && SearchEnemy())
            AutoTarget();


        if (!IsAttacking)
        {
            attackRoutine = StartCoroutine(Attack(spellIName));
        }
    }
    public void EquipItem(EquipmentItem newItem)
    {
        int partNum = 0;
        switch (newItem.GetPart)
        {
            case Part.Helmet:
                partNum = 0;
                break;
            case Part.Cloth:
                partNum = 1;
                break;
            case Part.Shoes:
                partNum = 2;
                break;
            case Part.Weapon:
                partNum = 3;
                break;
            case Part.Shoulder:
                partNum = 4;
                break;
            case Part.Back:
                partNum = 5;
                break;
        }
        if(usingEquipment[partNum] == null)
        {
            usingEquipment[partNum] = newItem;
            _spriteList.ChangeItem(partNum);
            useEquipment(partNum); // 델리게이트 실행
        }

    }
    //public override void TakeDamage(int damage, Transform source, Vector2 knockbackDir)
    //{
    //    base.TakeDamage(damage, knockbackDir);
    //}
}
