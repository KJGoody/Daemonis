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
        if (GameObject.FindWithTag("Enemy") == null)
            return false;
        else
            return true;
    }

    private void AutoTarget() // ���� ������� Ÿ����
    {
        if (FindNearestObject() != null)
            MyTarget = FindNearestObject().transform.Find("HitBox").transform;
        else
            MyTarget = null;
    }

    private GameObject FindNearestObject()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 7, LayerMask.GetMask("HitBox"));
        
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < collisions.Length; i++)
            if(collisions[i].CompareTag("Enemy"))       // �ױװ� ���ΰ��� ã�´�.
                objects.Add(collisions[i].gameObject);

        if (objects.Count == 0)     // ���� ����Ʈ�� ����ٸ� null�� ��ȯ
            return null;

        var neareastObject = objects        // �Ÿ��� ���� ª�� ������Ʈ�� ���Ѵ�.
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject.transform.parent.gameObject;
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

            case Spell.SpellLaunchType.AOE:
                if(MyTarget != null)        // Ÿ���� ���� �� ���� ���
                    return Instantiate(spell.MySpellPrefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
                else        // Ÿ���� ���� �� ���� ��ġ ����
                {
                    Vector3 ExitPoint = transform.position + new Vector3(atkDir.x, atkDir.y, 0).normalized * 2;
                    if(ExitPoint == transform.position)
                    {
                        if (_prefabs.transform.localScale.x == -1)
                            ExitPoint += new Vector3(1, 0, 0).normalized * 2;
                        else
                            ExitPoint += new Vector3(-1, 0, 0).normalized * 2;
                    }
                    return Instantiate(spell.MySpellPrefab, ExitPoint, Quaternion.identity).GetComponent<SpellScript>();
                }

            case Spell.SpellLaunchType.Toggle:
                return Instantiate(spell.MySpellPrefab, transform).GetComponent<SpellScript>(); 
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
}
