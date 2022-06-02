using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBase : NPC
{
    [HideInInspector]
    public EnemyType enemytype;
    private IState currentState;

    [HideInInspector]
    public Vector3 myStartPosition;
    [SerializeField]
    private GameObject HealthBarImage;
    public Transform ExitPoint;
    [HideInInspector]
    public float RubbingTime = 0f;

    [HideInInspector]
    public float myAggroRange;
    [HideInInspector]
    public float myAttackRange;

    [HideInInspector]
    public float MyAttackTime = 1000f;

    private bool IsKnockBack = false;

    private MonsterGate ParentGate;

    protected override void Awake()
    {
        enemytype = gameObject.GetComponent<EnemyType>();
        myStartPosition = transform.position;

        base.Awake();
    }

    protected override void Start()
    {
        ChangeState(new IdleState());
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
            currentState.Exit();

        currentState = newState;
        currentState.Enter(this);
    }

    public void ExitState(IState currentstate)
    {
        currentstate.Exit();
    }

    public override Transform Select()
    {
        HealthBarImage.SetActive(true);

        return base.Select();
    }

    public override void DeSelect()
    {
        HealthBarImage.SetActive(false);

        base.DeSelect();
    }

    public void CreateResource(GameObject resource, Transform transform, bool IsAttack = false)
    {
        GameObject EnemyAttackPrefab = Instantiate(resource, transform);
        if(IsAttack)
            EnemyAttackPrefab.GetComponent<EnemyAttack>().parent = this;
    }

    public override void TakeDamage(bool IsPhysic, float HitPercent, float PureDamage, int FromLevel, Vector2 knockbackDir, DamageTextPool.DamageTextPrefabsName TextType) // 피격
    {
        HealthBarImage.SetActive(true);
        if (knockbackDir != Vector2.zero)
            StartCoroutine(KnockBack(knockbackDir, 1));

        base.TakeDamage(IsPhysic, HitPercent, PureDamage, FromLevel, knockbackDir, TextType);

        if (stat.CurrentHealth <= 0)
        {
            ChangeState(new IdleState());

            _prefabs.PlayAnimation(2);
            HealthBarImage.SetActive(false);
            _prefabs.transform.GetChild(0).GetComponent<SortingGroup>().sortingLayerName = "DeathEnemyLayer";
            transform.Find("HitBox").gameObject.SetActive(false);
            transform.Find("EnemyBody").gameObject.SetActive(false);
            myRigid2D.simulated = false;

            ItemDropManager.MyInstance.DropGold(transform);
            ItemDropManager.MyInstance.DropItem(transform);

            StartCoroutine(Death());
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

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        SetLayersRecursively(_prefabs.transform, "None");
        yield return new WaitForSeconds(0.1f);
        SetLayersRecursively(_prefabs.transform, "Default");
        MonsterPool.Instance.ReturnObject(this);

        InitializeEnemyBase();
        ParentGate.CurrentEnemyNum--;
    }

    private void SetLayersRecursively(Transform Object, string name)
    {
        Object.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform Child in Object)
            SetLayersRecursively(Child, name);
    }

    public void PositioningEnemyBase(MonsterGate parentGate, Vector3 startPosition)
    {
        ParentGate = parentGate;
        myStartPosition = startPosition;
        transform.position = startPosition;
    }
    
    
    public void InitializeEnemyBase()
    {
        MyStat.InitializeHealth();
        _prefabs.transform.GetChild(0).GetComponent<SortingGroup>().sortingLayerName = "EnemyLayer";
        transform.Find("HitBox").gameObject.SetActive(true);
        transform.Find("EnemyBody").gameObject.SetActive(true);
        myRigid2D.simulated = true;
        MyTarget = null;
    }
}
