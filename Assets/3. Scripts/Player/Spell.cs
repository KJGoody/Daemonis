using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{


    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;

    private Transform target; // 공격할 대상


    // Use this for initialization
    void Start()
    {

        myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.Find("target").transform; // 타겟 이름 가진 게임오브젝트 찾기
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position; // 타겟과 스펠의 방향과 크기 구함
        myRigidbody.velocity = direction.normalized * speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Math.Atan2 탄젠트 값으로 각도를 산출 https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg 라디안 각도 변환해주는 상수 http://jw910911.tistory.com/6

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 축 중심 각도로 회전

    }
}