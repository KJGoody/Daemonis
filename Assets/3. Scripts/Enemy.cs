using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    private IState currentState;
    public float MyAttackRange { get; set; } // 사거리
    public float MyAttackTime { get; set; } // 공격 딜레이를 체크하기 위한 속성
    [SerializeField]
    private CanvasGroup healthGroup;

    //[SerializeField]
    //private Transform target;
    //public Transform Target
    //{
    //    get
    //    {
    //        return target;
    //    }

    //    set
    //    {
    //        target = value;
    //    }
    //}
    protected void Awake()
    {   
        MyAttackRange = 1; // 임시 코드
        ChangeState(new IdleState());
    }
    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }
            currentState.Update();
        }
        base.Update();
    }
    protected override void FixedUpdate()
    {
        //FollowTarget();
        base.FixedUpdate();
    }
    public override Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;

        return base.Select();
    }
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }


    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }
    public override void TakeDamage(int damage, Transform source)
    {
        base.TakeDamage(damage, source);
        //OnHealthChanged(health.MyCurrentValue);
    }

    //private void FollowTarget()
    //{
    //    if (target != null)
    //    {
    //        Vector2 targetPosition = target.position;
    //        Vector2 myPosition = transform.position;
    //        direction = targetPosition - myPosition;


    //        //transform.position = Vector2.MoveTowards(myPosition, targetPosition, speed * Time.deltaTime);
    //    }
    //    else{
    //        direction = Vector2.zero;
    //    }
    //}
}