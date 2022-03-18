using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;

    public Transform MyTarget { get; set; } // 공격할 대상

    private int damage;

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 direction = MyTarget.position - transform.position; // 타겟과 스펠의 방향과 크기 구함
        myRigidbody.velocity = direction.normalized * speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Math.Atan2 탄젠트 값으로 각도를 산출 https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg 라디안 각도 변환해주는 상수 http://jw910911.tistory.com/6

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 축 중심 각도로 회전

    }
    public void Initailize(Transform target, int damage)
    {
        this.MyTarget = target;
        this.damage = damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox") && collision.transform.position == MyTarget.position)
        {
            speed = 0;
            collision.GetComponentInParent<Enemy>().TakeDamage(damage);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
        }
    }
}