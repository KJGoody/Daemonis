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
    private Transform exitPoint; // �߻�ü ���� ��ġ

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
    public override void FindTarget()   //�����ϴ� Ÿ�� ���� �ٶ󺸱�
    {
        atkDir = Direction;
        base.FindTarget();
    }
    private IEnumerator Attack(string spellIName)
    {
        Spell newSpell = SpellBook.MyInstance.CastSpell(spellIName); //����Ͽ��� ��ų �޾ƿ�
        IsAttacking = true;
        _prefabs.PlayAnimation(4);
        if (MyTarget != null) // �ؿ��� �����Ѽ��ۿ� ����
        {
            FindTarget();
        }

        GameObject spell = newSpell.MySpellPrefab;

        SpellScript s = Instantiate(spell, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initailize(newSpell.MyDamage,transform,atkDir);

        if (MyTarget != null) //������ �����Ѽ��ۿ� ����
        {
            s.MyTarget = MyTarget;
        }
        
        yield return new WaitForSeconds(0.3f); // �׽�Ʈ�� ���� �ڵ��Դϴ�. ����ٰ� �ĵ������� ������
        StopAttack();
       
    }
    private void AutoTarget() // ���� ������� Ÿ����
    {
        MyTarget = FindNearestObjectByTag("HitBox").GetComponent<Transform>();
        if(Vector2.Distance(MyTarget.position,transform.position) > 7) // �ʹ� �ָ� Ÿ�� ����
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
    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            IsAttacking = false;
        }
    }
    public bool SearchEnemy() // ���� ���� �����ϴ��� �˻�
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
            useEquipment(partNum); // ��������Ʈ ����
        }

    }
    //public override void TakeDamage(int damage, Transform source, Vector2 knockbackDir)
    //{
    //    base.TakeDamage(damage, knockbackDir);
    //}
}
