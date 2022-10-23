using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBase : Character, INpc
{
    [HideInInspector] public EnemyType enemytype;
    protected IState currentState;

    [SerializeField] protected GameObject HealthBarImage;
    public Transform ExitPoint;

    [HideInInspector] public float myAttackRange;

    [HideInInspector] public float MyAttackTime = 1000f;

    private bool IsKnockBack = false;

    protected EnemySpawn ParentGate;

    protected override void Awake()
    {
        enemytype = GetComponent<EnemyType>();

        base.Awake();
    }

    protected override void Start()
    {
        ChangeState(new FollowState());
        // Base Set
        myAttackRange = enemytype.AttackRange;
        //-- Stat --
        MyStat.Level = enemytype.Level;
        MyStat.BaseAttack = Mathf.RoundToInt(enemytype.Attack * IngameManager.StatPercent);
        MyStat.BaseMaxHealth = Mathf.RoundToInt(enemytype.MaxHealth * IngameManager.StatPercent);
        MyStat.MoveSpeed = enemytype.MoveSpeed;
        MyStat.HitPercent = enemytype.HitPercent;
        MyStat.SetStat();

        base.Start();
    }

    protected virtual void OnEnable()
    {
        MyTarget = Player.MyInstance.transform;
    }

    protected override void Update()
    {
        if (InvadeGage.Instance.IsBossTime)
        {
            EnemyPool.Instance.ReturnObject(this, EnemyPool.Instance.GetIndex(GetComponent<EnemyType>().Prefab));
        }

        if (IsAlive)
        {
            if (!IsAttacking)
                MyAttackTime += Time.deltaTime;

            currentState.Update();
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (!IsKnockBack)
            base.FixedUpdate();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null) MyTarget = target;
    }

    public virtual Transform Select()
    {
        HealthBarImage.SetActive(true);
        return hitBox;
    }

    public virtual void DeSelect()
    {
        HealthBarImage.SetActive(false);
    }

    public void InstantiateAttack(GameObject resource, Transform transform)
    {
        GameObject EnemyAttackPrefab = Instantiate(resource, transform);
        EnemyAttackPrefab.GetComponent<EnemyAttack>().parent = this;
    }

    public override void TakeDamage(DamageType damageType, float HitPercent, float pureDamage, int FromLevel, Vector2 knockbackDir, NewTextPool.NewTextPrefabsName TextType, AttackType attackType = AttackType.Normal) // 피격
    {
        HealthBarImage.SetActive(true);
        if (knockbackDir != Vector2.zero)
            StartCoroutine(KnockBack(knockbackDir, 1));

        base.TakeDamage(damageType, HitPercent, pureDamage, FromLevel, knockbackDir, TextType);

        if (stat.CurrentHealth <= 0)
        {
            Player.MyInstance.SpendEXP(enemytype.EXP);
            ChangeState(new FollowState());

            SoundManager.Instance.PlaySFXSound("BodyExploding" + Random.Range(1, 3));
            _prefabs.PlayAnimation(2);
            HealthBarImage.SetActive(false);
            _prefabs.transform.GetChild(0).GetComponent<SortingGroup>().sortingLayerName = "DeathEnemyLayer";
            transform.Find("HitBox").gameObject.SetActive(false);
            transform.Find("EnemyBody").gameObject.SetActive(false);
            myRigid2D.simulated = false;

            ItemDropManager.MyInstance.DropGold(transform, Player.MyInstance.MyStat.Level);
            ItemDropManager.MyInstance.DropItem(transform, Player.MyInstance.MyStat.Level);

            StartCoroutine(Death());
            ComboManager.Instance.IncreaseCombo();
        }
    }


    protected IEnumerator KnockBack(Vector2 direction, float force) // 피격 시 넉백
    {
        IsKnockBack = true;
        myRigid2D.velocity = direction * force;
        yield return new WaitForSeconds(0.1f);
        myRigid2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        IsKnockBack = false;
    }


    protected virtual IEnumerator Death()
    {
        IsAlive = false;
        InvadeGage.Instance.CurrentValue += 1;
        ClearPanel.Instance.KillCount++;
        ParentGate.CurrentEnemyNum--;
        QuestPanel.Instance.CheckQuestGoal(QuestInfo.GoalTypes.Kill, enemytype.strEnemyType);

        yield return new WaitForSeconds(3f);
        SetLayersRecursively(_prefabs.transform, "None");

        yield return new WaitForSeconds(0.2f);
        SetLayersRecursively(_prefabs.transform, "Default");

        EnemyPool.Instance.ReturnObject(this, EnemyPool.Instance.GetIndex(GetComponent<EnemyType>().Prefab));

        InitializeEnemyBase();
    }

    protected void SetLayersRecursively(Transform Object, string name)
    {
        Object.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform Child in Object)
            SetLayersRecursively(Child, name);
    }

    public void PositioningEnemyBase(EnemySpawn parentGate, Vector3 startPosition)
    {
        ParentGate = parentGate;
        transform.position = startPosition;

        if (ChanceMaker.GetThisChanceResult_Percentage(50))
            SoundManager.Instance.PlaySFXSound(enemytype.Sound + Random.Range(1, 5));
    }

    public void InitializeEnemyBase()
    {
        IsAlive = true;
        MyStat.InitializeHealth();
        _prefabs.transform.GetChild(0).GetComponent<SortingGroup>().sortingLayerName = "EnemyLayer";
        transform.Find("HitBox").gameObject.SetActive(true);
        transform.Find("EnemyBody").gameObject.SetActive(true);
        myRigid2D.simulated = true;
        MyTarget = null;
    }
}
