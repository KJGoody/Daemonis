using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    [SerializeField]
    private float speed;
    protected Vector2 direction;

    private Rigidbody2D myRigid2D;
    public SPUM_Prefabs _prefabs;
    public SPUM_SpriteList _spriteList;

    protected bool isAttacking = false;
    protected Coroutine attackRoutine;
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
    public void Move()
    {
        if (isAttacking)
            myRigid2D.velocity = Vector2.zero;
        else
            myRigid2D.velocity = direction.normalized * speed;
        
    }
    public void HandleLayers()
    {
        if (IsMoving && !isAttacking)
        {
            _layerName = LayerName.move;
            _prefabs.PlayAnimation(1);

            // 캐릭터 좌우 보는거
            if (direction.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
            else if (direction.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(!IsMoving && !isAttacking)
        {
            _prefabs.PlayAnimation(0);
            _layerName = LayerName.idle;
        }
        if (isAttacking)
        {
           // _prefabs.PlayAnimation(4);
        }
    }
    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
        }
    }

}