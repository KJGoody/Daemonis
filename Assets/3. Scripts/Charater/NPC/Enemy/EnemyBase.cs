using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBase : Character, INpc
{
    [HideInInspector]
    public EnemyType enemytype;
    protected IState currentState;

    [HideInInspector]
    public Vector3 myStartPosition;
    [SerializeField]
    protected GameObject HealthBarImage;
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

    protected MonsterGate ParentGate;

    [SerializeField]
    protected int EnemyEXP;

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

        if(Vector2.Distance(transform.position, Player.MyInstance.transform.position) > 10)
        {
            ChangeState(new IdleState());
            switch (enemytype.enemyType)
            {
                case EnemyType.EnemyTypes.Koblod_Melee:
                    MonsterPool.Instance.ReturnObject(this, MonsterPool.MonsterPrefabName.Kobold_Melee);
                    break;

                case EnemyType.EnemyTypes.Koblod_Ranged:
                    MonsterPool.Instance.ReturnObject(this, MonsterPool.MonsterPrefabName.Kobold_Ranged);
                    break;
            }

            InitializeEnemyBase();
            ParentGate.CurrentEnemyNum--;
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
            Player.MyInstance.SpendEXP(EnemyEXP);
            ChangeState(new IdleState());

            SoundManager.Instance.PlaySFXSound("BodyExploding" + Random.Range(1, 3));
            _prefabs.PlayAnimation(2);
            HealthBarImage.SetActive(false);
            _prefabs.transform.GetChild(0).GetComponent<SortingGroup>().sortingLayerName = "DeathEnemyLayer";
            transform.Find("HitBox").gameObject.SetActive(false);
            transform.Find("EnemyBody").gameObject.SetActive(false);
            myRigid2D.simulated = false;

            ItemDropManager.MyInstance.DropGold(transform, stat.Level);
            ItemDropManager.MyInstance.DropItem(transform, stat.Level);

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

    public void SetTarget(Transform target)
    {
        if (MyTarget == null)
        {
            MyTarget = target;
        }
    }

    protected virtual IEnumerator Death()
    {
        InvadeGage.Instance.CurrentValue += 1;

        yield return new WaitForSeconds(3f);
        SetLayersRecursively(_prefabs.transform, "None");

        yield return new WaitForSeconds(0.2f);
        SetLayersRecursively(_prefabs.transform, "Default");

        switch (enemytype.enemyType)
        {
            case EnemyType.EnemyTypes.Koblod_Melee:
                MonsterPool.Instance.ReturnObject(this, MonsterPool.MonsterPrefabName.Kobold_Melee);
                break;

            case EnemyType.EnemyTypes.Koblod_Ranged:
                MonsterPool.Instance.ReturnObject(this, MonsterPool.MonsterPrefabName.Kobold_Ranged);
                break;
        }

        InitializeEnemyBase();
        ParentGate.CurrentEnemyNum--;
        ParentGate.DeathEnemyNum++;
    }

    protected void SetLayersRecursively(Transform Object, string name)
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

        if(ChanceMaker.GetThisChanceResult_Percentage(50))
            SoundManager.Instance.PlaySFXSound("MoiusesquealSound" + Random.Range(1, 5));
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
