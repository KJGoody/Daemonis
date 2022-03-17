using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : MonoBehaviour
{
    public FloatingJoystick joy;
    Vector3 moveVec;
    public float moveSpeed = 30;
    public float moveForce = 10f;
    public float maxSpeed = 500;

    public SPUM_Prefabs _prefabs;
    public SPUM_SpriteList _spriteList;
    public float _charMS;
    public enum PlayerState
    {
        idle,
        move,
        attack,
        death,
    }
    public PlayerState _playerState = PlayerState.idle;
    public Vector3 _goalPos;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {

    }
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.y * 0.01f); // 이건 먼지 모름 한번 지워봐야함

        // 조이스틱에서 bool값 가져와서 이동 기본상태 나눠줌
        if (joy.onMove)
        {
            _playerState = PlayerState.move;
            _prefabs.PlayAnimation(1);
        }
        else
        {
            _prefabs.PlayAnimation(0);
            _playerState = PlayerState.idle;
        }
        if (Input.GetMouseButtonDown(1))
        {

            _spriteList.ChangeItem();
        }
    }
    private void FixedUpdate()
    {
        switch (_playerState)
        {
            case PlayerState.idle:
                Rigidbody2D rd2d = gameObject.GetComponent<Rigidbody2D>();
                rd2d.velocity = Vector2.zero;
                break;

            case PlayerState.move:
                DoMove();
                break;
        }
    }
    void DoMove()
    {
        float x = joy.Horizontal;
        float y = joy.Vertical;
        //transform.Translate(new Vector2(x, y).normalized * moveSpeed * Time.deltaTime);
        Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();

        rb2d.velocity = new Vector2( x , y ) * maxSpeed * Time.deltaTime;
        


        //if(!joy.onMove)
        //{
        //    _prefabs.PlayAnimation(0);
        //    _playerState = PlayerState.idle;
        //    return;
        //}
        //Vector3 _dirVec  = _goalPos - transform.position ;
        //Vector3 _disVec = (Vector2)_goalPos - (Vector2)transform.position ;
        //if( _disVec.sqrMagnitude < 0.1f )
        //{
        //    _prefabs.PlayAnimation(0);
        //    _playerState = PlayerState.idle;
        //    return;
        //}
        //Vector3 _dirMVec = _dirVec.normalized;
        //transform.position += (_dirMVec * _charMS * Time.deltaTime );


        if (x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetMovePos(Vector2 pos)
    {
        _goalPos = pos;
        _playerState = PlayerState.move;
        _prefabs.PlayAnimation(1);
    }
}
