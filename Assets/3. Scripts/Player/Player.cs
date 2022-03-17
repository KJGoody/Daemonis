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
            if (!isAttacking) // 유투브 버전에 버그가 있어, 임의로 제가 추가한 코드입니다.
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
        yield return new WaitForSeconds(0.3f); // 테스트를 위한 코드입니다. 여기다가 후딜넣을까 생각중
        StopAttack();
       
    }
    public void CastSpell()
    {
        Instantiate(spellPrefab[0], transform.position, Quaternion.identity);

    }
}
