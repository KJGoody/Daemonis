using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticleSpell : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private ParticleSystem particleObject;
    [SerializeField]
    private float speed;
    public Transform MyTarget { get; set; } // 공격할 대상
    private Transform source;
    private int damage;
    private Vector2 direction;
    List<GameObject> hitEnemy = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        particleObject = GetComponent<ParticleSystem>();
        direction = MyTarget.position - transform.position;
    }

    private void FixedUpdate()
    {
        //Vector2 direction = MyTarget.position - transform.position; // 타겟과 스펠의 방향과 크기 구함
        myRigidbody.velocity = direction.normalized * speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Math.Atan2 탄젠트 값으로 각도를 산출 https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg 라디안 각도 변환해주는 상수 http://jw910911.tistory.com/6

        particleObject.startRotation = angle;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 축 중심 각도로 회전

    }
    public void Initailize(Transform target, int damage, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox"))// && collision.transform.position == MyTarget.position 원래코드 삭제 (유도기능 넣을때 추가하면 좋을듯)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            if (!CheckHitEnemy(collision))
                c.TakeDamage(damage, direction, source); // 피격된 대상에게 자신의 위치 정보 전달
            GetComponent<Animator>().SetTrigger("impact");
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
        }
    }
    private bool CheckHitEnemy(Collider2D collision) // 스킬 한번 맞았으면 다시 안맞게 체크
    {
        GameObject g = collision.GetComponent<GameObject>();
        if (!hitEnemy.Contains(g))
        {
            hitEnemy.Add(g);
            return false;
        }
        else
            return true;

    }

}
