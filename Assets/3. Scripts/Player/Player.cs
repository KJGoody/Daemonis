using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private FloatingJoystick joy;

    [SerializeField]
    private Stat mana;
    private float initMana = 50;

    [SerializeField]
    private Transform exitPoint; // �߻�ü ���� ��ġ

    private SpellBook spellBook;

    public Transform myTarget { get; set; }
    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
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
        if (!isAttacking)
        {
            moveVector.x = joy.Horizontal;
            moveVector.y = joy.Vertical;

            Direction = moveVector;
        }
         
    }
    private void FindTarget()
    {
        Direction = myTarget.position - transform.position;
        if (Direction.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (Direction.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }
    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = myTarget;
        Spell newSpell = spellBook.CastSpell(spellIndex); //����Ͽ��� ��ų �޾ƿ�

        isAttacking = true;
        FindTarget();
        _prefabs.PlayAnimation(4);

        GameObject spell = newSpell.MySpellPrefab;
        if (currentTarget != null)
        {
            SpellScript s = Instantiate(spell, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initailize(currentTarget, newSpell.MyDamage);
            s.MyTarget = myTarget;
        }
        yield return new WaitForSeconds(0.3f); // �׽�Ʈ�� ���� �ڵ��Դϴ�. ����ٰ� �ĵ������� ������
        StopAttack();
       
    }
    public void CastSpell(int spellIndex)
    {
        if (myTarget == null) return;
        if (!isAttacking)
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
        }
    }
}
