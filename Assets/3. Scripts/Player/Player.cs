using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private FloatingJoystick joy;
    [SerializeField]
    private Stat health;
    [SerializeField]
    private Stat mana;

    private float initHealth = 100;
    private float initMana = 50;

    [SerializeField]
    private GameObject[] spellPrefab;

    protected override void Start()
    {
        joy = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();
        health.Initialize(initHealth, initHealth);
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

        moveVector.x = joy.Horizontal;
        moveVector.y = joy.Vertical;

        direction = moveVector;
         
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking) // ������ ������ ���װ� �־�, ���Ƿ� ���� �߰��� �ڵ��Դϴ�.
            {
                attackRoutine = StartCoroutine(Attack());
            }
        }
    }
    private IEnumerator Attack()
    {
        isAttacking = true;
        _prefabs.PlayAnimation(4);
        CastSpell();
        yield return new WaitForSeconds(0.3f); // �׽�Ʈ�� ���� �ڵ��Դϴ�. ����ٰ� �ĵ������� ������
        StopAttack();
       
    }
    public void CastSpell()
    {
        Instantiate(spellPrefab[0], transform.position, Quaternion.identity);

    }
}
