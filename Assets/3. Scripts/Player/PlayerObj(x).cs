using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// 
///  @@ 강의 따라 하기 전 SPUM만 있을 때 사용한 스크립트임
///  @@ 2022 03 17 강의에 맞게 Player와 Character 스크립트로 분리함
///  @@ 어떤 상황이 발생할지 몰라 일단 뒀지만 나중에 삭제하게될 스크립트임
/// 
/// 
/// </summary>
public class PlayerObj : MonoBehaviour
{
    public FloatingJoystick joy;
    public float speed = 500;

    public SPUM_Prefabs _prefabs;
    public SPUM_SpriteList _spriteList;
    //public float _charMS;
    public enum PlayerState
    {
        idle,
        move,
        attack,
        death,
    }
    public PlayerState _playerState = PlayerState.idle;
    //public Vector3 _goalPos;
    // Start is called before the first frame update

    // Update is called once per frame
    protected virtual void Start()
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

        // 테스트용
        if (Input.GetMouseButtonDown(1))
        {

            //_spriteList.ChangeItem();
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
        Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2( x , y ) * speed * Time.deltaTime;

        if (x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }
}
