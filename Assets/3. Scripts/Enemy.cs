using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    private IState currentState;
    public Vector3 MyStartPosition { get; set; } // 시작 위치
    public float MyAttackRange { get; set; } // 사거리
    public float MyAttackTime { get; set; } // 공격 딜레이를 체크하기 위한 속성
    [SerializeField]
    private CanvasGroup healthGroup;
    private bool isKnockBack;
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
        if(!isKnockBack)
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
        healthGroup.alpha = 1;
        StartCoroutine(KnockBack(new Vector2(1,1),5));
        //KnockBack(new Vector2(1, 0), 5);
        SetTarget(source);
        base.TakeDamage(damage, source);
        if (health.MyCurrentValue <= 0)
        {
            //GameObject hitbox = transform.Find("HitBox").gameObject;
            Destroy(transform.Find("HitBox").gameObject);
        }
        //OnHealthChanged(health.MyCurrentValue);
    }
    //public void KnockBack(Vector2 direction, float force)
    //{
    //    myRigid2D.velocity = direction * force;
    //    Debug.Log("넉백");
    //}
    IEnumerator KnockBack(Vector2 direction, float force)
    {
        isKnockBack = true;
        myRigid2D.velocity = direction * force;
        yield return new WaitForSeconds(0.1f);
        myRigid2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        isKnockBack = false;
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