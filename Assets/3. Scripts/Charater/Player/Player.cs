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
    private Transform exitPoint; // �߻�ü ���� ��ġ

    public Vector2 atkDir;

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
        {
            attackRoutine = StartCoroutine(Attack(spellIName));
        }
    }

    private bool SearchEnemy() // ���� ���� �����ϴ��� �˻�
    {
        if (GameObject.FindWithTag("HitBox") == null)
            return false;
        else
            return true;
    }

    private void AutoTarget() // ���� ������� Ÿ����
    {
        MyTarget = FindNearestObjectByTag("HitBox").GetComponent<Transform>();
        if (Vector2.Distance(MyTarget.position, transform.position) > 7) // �ʹ� �ָ� Ÿ�� ����
        {
            MyTarget = null;
        }
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

    private IEnumerator Attack(string spellIName)
    {
        IsAttacking = true;
        _prefabs.PlayAnimation(4);  // ���� �ִϸ��̼� ���
        if (MyTarget != null) // �ؿ��� �����Ѽ��ۿ� ����
            FindTarget();

        Spell newSpell = SpellBook.MyInstance.GetSpell(spellIName); //����Ͽ��� ��ų �޾ƿ�
        SpellScript spellScript = InstantiateSpell(newSpell);
        spellScript.atkDir = atkDir;
        if (MyTarget != null) //������ �����Ѽ��ۿ� ����
            spellScript.MyTarget = MyTarget;

        yield return new WaitForSeconds(spellScript.CastTime); // �׽�Ʈ�� ���� �ڵ��Դϴ�. ����ٰ� �ĵ������� ������
        StopAttack();
    }

    public override void FindTarget()   //�����ϴ� Ÿ�� ���� �ٶ󺸱�
    {
        atkDir = Direction;
        base.FindTarget();
    }

    private SpellScript InstantiateSpell(Spell spell)
    {
        switch (spell.spellLaunchType)
        {
            case Spell.SpellLaunchType.Launch:
                return Instantiate(spell.MySpellPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();

            case Spell.SpellLaunchType.AE:
                return Instantiate(spell.MySpellPrefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();

            case Spell.SpellLaunchType.AOE:
                return Instantiate(spell.MySpellPrefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
        }

        return null;
    }

    private void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            IsAttacking = false;
        }
    }
    
    //public override void TakeDamage(int damage, Transform source, Vector2 knockbackDir)
    //{
    //    base.TakeDamage(damage, knockbackDir);
    //}
}
