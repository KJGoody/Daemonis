using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    
    [SerializeField]
    private float speed;
    private Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }
    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    protected Rigidbody2D myRigid2D;
    public SPUM_Prefabs _prefabs;
    public SPUM_SpriteList _spriteList;

    public Transform MyTarget { get; set; }
    public bool IsAttacking { get; set; }
    protected Coroutine attackRoutine;


    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    [SerializeField]
    private float initHealth;
    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }
    public enum LayerName
    {
        idle = 0,
        move = 1,
        attack = 4,
        death = 2,
    }
    public LayerName _layerName = LayerName.idle;
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);
        myRigid2D = gameObject.GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {
        HandleLayers();
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }
    public virtual void Move()
    {
        if (IsAlive)
        {
            if (IsAttacking)
                myRigid2D.velocity = Vector2.zero;
            else
                myRigid2D.velocity = direction.normalized * speed;
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
            if (IsAttacking)
            {
                _layerName = LayerName.attack;
                //_prefabs.PlayAnimation(4);
            }
        }
        else
        {
            _layerName = LayerName.death;
        }
    }

    public void FindTarget()
    {
        Direction = MyTarget.position - transform.position;
        if (Direction.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (Direction.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }

    public virtual void TakeDamage(int damage, Transform source , Vector2 knockbackDir)
    {
        DamageText(damage);
        health.MyCurrentValue -= damage;
        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigid2D.velocity = direction;
        }
    }

    private void DamageText(int damage)
    {
        GameObject damageTxt = Instantiate(Resources.Load("DamageText/DamageText") as GameObject, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.identity);
        damageTxt.GetComponent<DamageText>().Damage = damage;
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

}