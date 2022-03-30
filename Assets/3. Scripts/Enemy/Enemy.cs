using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    private IState currentState;
    public enum EnemyType
    {
        kobold_melee,
        kobold_ranged
    }
    public EnemyType enemyType;
    private float myAttackRange;        // ��Ÿ�
    public float MyAttackRange {        
        get
        {
            return myAttackRange;
        }
        set
        {
            myAttackRange = value;
        } 
    }

    public GameObject EnemyAttackBox;            // �ִϹ̰��� �ڽ�
    public Vector3 MyStartPosition { get; set; } // ���� ��ġ
    public float MyAttackTime { get; set; } // ���� �����̸� üũ�ϱ� ���� �Ӽ�
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

    protected void Awake()
    {
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;

        switch (enemyType)                  // �ִϹ� Ÿ�Կ� ���� ���� ��Ÿ� ��ȭ
        {
            case EnemyType.kobold_melee:
                MyAttackRange = 1;
                break;

            case EnemyType.kobold_ranged:
                MyAttackRange = 5;
                break;
        }
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
    public override void TakeDamage(int damage, Transform source, Vector2 knockbackDir) // �ǰ�
    {
        healthGroup.alpha = 1;
        StartCoroutine(KnockBack(knockbackDir,1));
        SetTarget(source);
        base.TakeDamage(damage, source, knockbackDir);
        if (health.MyCurrentValue <= 0)
        {
            Destroy(transform.Find("HitBox").gameObject);
        }
        //OnHealthChanged(health.MyCurrentValue);
    }
    IEnumerator KnockBack(Vector2 direction, float force) // �ǰ� �� �˹�
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
}