using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : NPC
{
    private IState currentState;

    public enum EnemyType               // EnemyType
    {
        Basemelee,
        Baseranged,
        Baserush,
        BaseAOE
    }
    public EnemyType enemyType;

    public Vector3 MyStartPosition { get; set; } // 시작 위치
    public float MyAttackTime { get; set; } // 공격 딜레이를 체크하기 위한 속성
    [SerializeField]
    private CanvasGroup healthGroup;
    private bool isKnockBack;
    [SerializeField]
    private float initAggroRange;
    public Transform exitPoint;         // 발사체 생성 위치
    private float myAttackRange;        // 사거리
    public float MyAttackRange
    {
        get { return myAttackRange; }
        set { myAttackRange = value; }
    }
    public Rigidbody2D myrigid2D
    {
        get { return myRigid2D; }
        set { myrigid2D = value; }
    }
    public float MyAggroRange { get; set; }
    public bool InRange
    {
        get { return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange; }
    }
    public GameObject unitroot;

    private ComboManager comboManager;
    private bool IsComboNPC;

    public NavManager navManager;

    protected void Awake()
    {
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;

        comboManager = FindObjectOfType<ComboManager>();
        navManager = FindObjectOfType<NavManager>();

        switch (enemyType)                  // 애니미 타입에 따라 공격 사거리 변화
        {
            case EnemyType.Basemelee:
                MyAttackRange = 1;
                IsComboNPC = true;
                break;

            case EnemyType.Baseranged:
                MyAttackRange = 5;
                IsComboNPC = true;
                break;

            case EnemyType.Baserush:
                MyAttackRange = 5;
                IsComboNPC = true;
                break;

            case EnemyType.BaseAOE:
                MyAttackRange = 5;
                IsComboNPC = true;
                break;
        }
        MyAttackTime = 1000f;            // 공격상태 돌입 시 바로 공격할 수 있도록 시간을 많이 넣어둠
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
        if (!isKnockBack)
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

    public void EnemyAttackResource(GameObject EAR, Vector3 vector, Quaternion quaternion)
    {
        Instantiate(EAR, vector, quaternion);
    }

    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }
    public override void TakeDamage(int damage, Vector2 knockbackDir, Transform source = null, string TextType = null) // 피격
    {
        healthGroup.alpha = 1;
        StartCoroutine(KnockBack(knockbackDir, 1));
        SetTarget(source);
        base.TakeDamage(damage, knockbackDir, null, TextType);
        if (health.MyCurrentValue <= 0)
        {
            _prefabs.PlayAnimation(2);
            healthGroup.alpha = 0;
            unitroot.GetComponent<SortingGroup>().sortingLayerName = "DeathEnemyLayer";
            Destroy(transform.Find("HitBox").gameObject);
            Destroy(transform.Find("EnemyBody").gameObject);

            StartCoroutine("Death");
            if (IsComboNPC)
                comboManager.IncreaseCombo();
        }
        //OnHealthChanged(health.MyCurrentValue);
    }
    IEnumerator KnockBack(Vector2 direction, float force) // 피격 시 넉백
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