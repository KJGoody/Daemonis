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
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    private FloatingJoystick joy;

    [SerializeField]
    private Stat mana;
    private float initMana = 50;

    [SerializeField]
    private Transform exitPoint; // 발사체 생성 위치

    private Vector2 atkDir;
    //public Transform myTarget { get; set; }
    protected override void Start()
    {
        joy = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();

        mana.Initialize(initMana, initMana);
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
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }

        Vector2 moveVector;
        if (!IsAttacking)
        {
            moveVector.x = joy.Horizontal;
            moveVector.y = joy.Vertical;
            Direction = moveVector;
            if(moveVector.x != 0 && moveVector.y != 0)
                atkDir = moveVector;
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

        GameObject spell = newSpell.MySpellPrefab;

        SpellScript s = Instantiate(spell, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initailize(newSpell.MyDamage,transform,atkDir);
        if (MyTarget != null)
        {
            FindTarget();
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
        Debug.Log("AutoTarget" + MyTarget);
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
        Debug.Log(GameObject.FindWithTag("HitBox"));
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

    //public override void TakeDamage(int damage, Transform source, Vector2 knockbackDir)
    //{
    //    base.TakeDamage(damage, knockbackDir);
    //}
}
