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
    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }
    public bool IsAttacking { get; set; }

        // 스탯
    [SerializeField]
    protected Stat stat;
    public Stat MyHealth    // 체력
    {
        get { return stat; }
    }

    public bool IsAlive
    {
        get { return stat.CurrentHealth > 0; }
    }

        // 이동관련
    private Vector2 direction;
    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    public Transform MyTarget { get; set; }

    protected Coroutine attackRoutine;

    public bool IsRushing { get; set; }
    [HideInInspector]
    public float RushSpeed = 0f;

    public List<Buff> onBuff = new List<Buff>();

    protected virtual void Awake()
    {

    }
    
    protected virtual void Start()
    {
        myRigid2D = gameObject.GetComponent<Rigidbody2D>();
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
                myRigid2D.velocity = direction.normalized * stat.Speed;
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
    public virtual void FindTarget()
    {
        Direction = MyTarget.position - transform.position;
        if (Direction.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (Direction.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }

    public void NewBuff(string buffName)
    {
        switch (buffName)
        {
            case "BaseBuff":
                StartBuff(buffName);
                break;
        }
    }

    public void StartBuff(string buffName)
    {
        if(onBuff.Count > 0)
        {
            for(int i = 0; i < onBuff.Count; i++)
            {
                if (onBuff[i].BuffName.Equals(buffName))
                {
                    onBuff[i].ResetBuff();
                }
                else
                {
                    BuffManager.myInstance.AddBuffImage(this);
                }
            }
        }
        else
        {
            BuffManager.myInstance.AddBuffImage(this);
        }
    }

    public virtual void TakeDamage(int damage, Vector2 knockbackDir, Transform source = null, string TextType = null)
    {
        NewDamageText(damage, TextType);
        stat.CurrentHealth -= damage;
        if (stat.CurrentHealth <= 0)
        {
            Direction = Vector2.zero;
            myRigid2D.velocity = direction;
        }
    }

    private void NewDamageText(int damage, string tagName)
    {
        GameObject newText = Instantiate(Resources.Load("NewText") as GameObject, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.identity);
        newText.GetComponent<DamageText>().TextType = tagName;
        newText.GetComponent<DamageText>().Damage = damage;
    }

    IEnumerator Death()
    {
        myRigid2D.simulated = false;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}