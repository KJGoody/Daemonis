using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    [SerializeField]
    private float speed;

    protected Vector2 direction;

    public SPUM_Prefabs _prefabs;
    public SPUM_SpriteList _spriteList;
    public enum PlayerState
    {
        idle,
        move,
        attack,
        death,
    }
    public PlayerState _playerState = PlayerState.idle;
    protected virtual void Start()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        Move();

    }


    public void Move()
    {
        Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.velocity = direction * speed * Time.deltaTime;

        if (direction.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
        
        if (direction.x != 0 || direction.y != 0)
        {
            _playerState = PlayerState.move;
            _prefabs.PlayAnimation(1);
        }
        else
        {
            _prefabs.PlayAnimation(0);
            _playerState = PlayerState.idle;
        }
    }

}