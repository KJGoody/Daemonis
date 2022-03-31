using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private Stat mana;
    private float initMana = 50;

    [SerializeField]
    private Transform exitPoint; // �߻�ü ���� ��ġ

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
        }
         
    }
    private void FindTarget()
    {
        Direction = MyTarget.position - transform.position;
        if (Direction.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (Direction.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }
    private IEnumerator Attack(string spellIName)
    {
        Transform currentTarget = MyTarget;
        Spell newSpell = SpellBook.MyInstance.CastSpell(spellIName); //����Ͽ��� ��ų �޾ƿ�

        IsAttacking = true;
        FindTarget();
        _prefabs.PlayAnimation(4);

        GameObject spell = newSpell.MySpellPrefab;
        if (currentTarget != null)
        {
            TestParticleSpell s = Instantiate(spell, exitPoint.position, Quaternion.identity).GetComponent<TestParticleSpell>();
            s.Initailize(currentTarget, newSpell.MyDamage,transform);
            s.MyTarget = MyTarget;
        }
        yield return new WaitForSeconds(0.3f); // �׽�Ʈ�� ���� �ڵ��Դϴ�. ����ٰ� �ĵ������� ������
        StopAttack();
       
    }
    private void AutoTarget()
    {
        MyTarget = FindNearestObjectByTag("HitBox").GetComponent<Transform>();
        
        Debug.Log("AutoTarget" + MyTarget);
    }
    private GameObject FindNearestObjectByTag(string tag)
    {
        // Ž���� ������Ʈ ����� List �� �����մϴ�.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
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

    public void CastSpell(string spellIName)
    {
        if (MyTarget == null)
            AutoTarget();

        Character EnemyCharacter = MyTarget.GetComponentInParent<Character>();
        if (!IsAttacking && EnemyCharacter.IsAlive)
        {
            attackRoutine = StartCoroutine(Attack(spellIName));
        }
    }
}
