using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBase : NPC
{
    public EnemyType enemytype;
    private IState currentState;

    [HideInInspector]
    public Vector3 myStartPosition;
    [SerializeField]
    private CanvasGroup healthGroup;
    public GameObject unitroot;
    public Transform ExitPoint;
    public float RubbingTime = 0f;

    [HideInInspector]
    public float myAggroRange;
    [HideInInspector]
    public float myAttackRange;
    
    [HideInInspector]
    public float MyAttackTime = 1000f;

    private bool IsKnockBack = false;


    protected override void Awake()
    {
        ChangeState(new IdleState());
        myStartPosition = transform.position;

        base.Awake();
    }

    protected override void Start()
    {
        myAggroRange = enemytype.AggroRnage;
        myAttackRange = enemytype.AttackRnage;

        base.Start();
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
        if (!IsKnockBack) 
        {
            base.FixedUpdate();
        }
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

    public override Transform Select()
    {
        healthGroup.alpha = 1;

        return base.Select();
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    public void CreateResource(GameObject resource, Transform transform)
    {
        Instantiate(resource, transform);
    }

    public override void TakeDamage(int damage, Vector2 knockbackDir, Transform source = null, string TextType = null) // 피격
    {
        healthGroup.alpha = 1;
        StartCoroutine(KnockBack(knockbackDir, 1));
        //SetTarget(source);
        base.TakeDamage(damage, knockbackDir, null, TextType);

        if (stat.CurrentHealth <= 0)
        {
            _prefabs.PlayAnimation(2);
            healthGroup.alpha = 0;
            unitroot.GetComponent<SortingGroup>().sortingLayerName = "DeathEnemyLayer";
            Destroy(transform.Find("HitBox").gameObject);
            Destroy(transform.Find("EnemyBody").gameObject);

            StartCoroutine("Death");
            ComboManager.Instance.IncreaseCombo();
        }
    }

    IEnumerator KnockBack(Vector2 direction, float force) // 피격 시 넉백
    {
        IsKnockBack = true;
        myRigid2D.velocity = direction * force;
        yield return new WaitForSeconds(0.1f);
        myRigid2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        IsKnockBack = false;
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            myAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.Direction = Vector2.zero;
    }


}
