using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    protected Rigidbody2D myRigid2D;
    public SPUM_Prefabs _prefabs;
    public SPUM_SpriteList _spriteList;
    [SerializeField]
    protected Transform hitBox; // 캐릭터 히트박스

    // 애니메이션
    public enum LayerName { idle = 0, move = 1, attack = 4, death = 2, }
    public LayerName _layerName = LayerName.idle;

    public bool IsAlive { get { return stat.CurrentHealth > 0; } }      // 생존 확인
    // 상태확인
    public bool IsMoving { get { return direction.x != 0 || direction.y != 0; } }
    public bool IsAttacking { get; set; }

    // 스탯
    protected Stat stat;
    public Stat MyStat { get { return stat; } }   // 스탯 가져오기

    // 이동관련
    private Vector2 direction;
    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    [HideInInspector]
    public Transform MyTarget;

    [HideInInspector]
    public bool IsRushing;
    [HideInInspector]
    public float RushSpeed = 0f;

    [SerializeField]
    private BuffManager buffManager;
    [HideInInspector]
    public List<Buff> OnBuff = new List<Buff>();
    [HideInInspector]
    public float BuffxDamage = 1;
    private float DebuffxDamage = 1;

    protected virtual void Awake()
    {
        stat = gameObject.GetComponent<Stat>();
        myRigid2D = gameObject.GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleLayers();
    }

    protected virtual void FixedUpdate()
    {
        Move(RushSpeed);
    }

    public virtual void Move(float RushSpeed = 0f)
    {
        if (IsAlive)
        {
            if (IsAttacking)
            {
                if (IsRushing)                                           // 돌진공격일시 이동 가능
                {
                    myRigid2D.velocity = Direction.normalized * RushSpeed;
                }
                else
                    myRigid2D.velocity = Vector2.zero;
            }
            else
                myRigid2D.velocity = direction.normalized * stat.MoveSpeed;
        }
    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
            // 캐릭터 좌우 보는거
            if (direction.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
            else if (direction.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);

            if (IsMoving && !IsAttacking)
            {
                _layerName = LayerName.move;
                _prefabs.PlayAnimation(1);
            }
            else if (!IsMoving && !IsAttacking)
            {
                _prefabs.PlayAnimation(0);
                _layerName = LayerName.idle;
            }

            if (IsRushing)
            {
                _layerName = LayerName.move;
                _prefabs.PlayAnimation(1);

            }
            else if (IsAttacking)
            {
                _layerName = LayerName.attack;
            }
        }
        else
        {
            _layerName = LayerName.death;
        }
    }

    public virtual void LookAtTarget()
    {
        if ((MyTarget.transform.position - transform.position).x > 0)
            _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if ((MyTarget.transform.position - transform.position).x < 0)
            _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }

    public void NewBuff(string buffName)
    {
        StartBuff(buffName);
    }

    private void StartBuff(string buffName)
    {
        if (OnBuff.Count > 0)
        {
            foreach (Buff buff in OnBuff)
            {
                if (buff.BuffName.Equals(buffName))
                    buff.ResetBuff();
                else
                    buffManager.AddBuffImage(buffName, this);
            }
        }
        else
            buffManager.AddBuffImage(buffName, this);
    }

    public void OffBuff(string buffName)
    {
        for (int i = OnBuff.Count - 1; i >= 0; i--)
        {
            if (OnBuff[i].BuffName.Equals(buffName))
                OnBuff[i].DeActivationBuff();
        }
    }

    public bool IsOnBuff(string buffName)
    {
        if (OnBuff.Count > 0)
        {
            foreach (Buff buff in OnBuff)
            {
                if (buff.BuffName.Equals(buffName))
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    public Buff GetBuff(string buffName)
    {
        if (IsOnBuff(buffName))
        {
            foreach (Buff buff in OnBuff)
            {
                if (buff.BuffName.Equals(buffName))
                    return buff;
            }
            return null;
        }
        else
            return null;
    }

    public virtual void TakeDamage(bool IsPhysic, float HitPercent, float pureDamage, int FromLevel, Vector2 knockbackDir, DamageTextPool.DamageTextPrefabsName TextType)
    {
        if (Random.value < HitPercent - MyStat.DodgePercent)
        {
            float PureDamage = pureDamage * DebuffxDamage;
            int Damage;
            if (IsPhysic)
                Damage = (int)Mathf.Floor((PureDamage * (PureDamage / (PureDamage + stat.BaseDefence + 1)) + (Random.Range(-pureDamage, pureDamage) / 10)) * LevelGapxDamage(FromLevel, MyStat.Level));
            else
                Damage = (int)Mathf.Floor((PureDamage * (PureDamage / (PureDamage + stat.BaseMagicRegist + 1)) + (Random.Range(-pureDamage, pureDamage) / 10)) * LevelGapxDamage(FromLevel, MyStat.Level));
            stat.CurrentHealth -= Damage;

            NewDamageText(TextType, Damage);
            if (stat.CurrentHealth <= 0)
            {
                Direction = Vector2.zero;
                myRigid2D.velocity = direction;
            }
        }
        else
            NewDamageText(TextType);
    }

    private float LevelGapxDamage(int FromLevel, int ToLevel)
    {
        int LevelGap = ToLevel - FromLevel;
        float xDamage = 1;

        if (LevelGap < -10)     xDamage = 1.3f;
        else if (LevelGap < 0)
        {
            for (int i = 0; i < -LevelGap; i++)
                xDamage += 0.025f;
        }
        else if (LevelGap < 3)  xDamage = 1;
        else if (LevelGap == 3) xDamage = 0.95f;
        else if (LevelGap == 4) xDamage = 0.9f;
        else
        {
            xDamage = 0.9f;
            for (int i = 4; i < LevelGap; i++)
                xDamage -= 0.025f;
        }
        return xDamage > 0 ? xDamage : 0;
    }


    private void NewDamageText(DamageTextPool.DamageTextPrefabsName TextType, int damage = 0)
    {
        DamageText damageText = DamageTextPool.Instance.GetObject(TextType);
        damageText.InitializeDamageText();
        damageText.PositioningDamageText(transform);
        damageText.Damage = damage;
    }
}