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

    [SerializeField]
    private float initAggroRange;
    public float MyAggroRange { get; set; }
    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }


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
        MyAggroRange = initAggroRange;
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
        SetTarget(source);
        base.TakeDamage(damage, source);
        //OnHealthChanged(health.MyCurrentValue);
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.Direction = Vector2.zero;
        //this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
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