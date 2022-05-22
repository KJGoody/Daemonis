using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private GameObject puff;
    
    public enum SpellType
    {
        Skill_File_01,
        Skill_File_02,
        Skill_File_03,
        Skill_File_04,
        Skill_File_05,
        Skill_File_06,
        Skill_File_07,
        Skill_File_08,
        Skill_File_09
    }
    [SerializeField]
    private SpellType spellType;

    public Transform MyTarget { get; set; } // 공격할 대상
    private Vector2 direction;
    public Vector2 atkDir;

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;

    private List<GameObject> hitEnemy = new List<GameObject>();

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        if (MyTarget != null)
            direction = MyTarget.position - transform.position;
        else
        {
            direction = atkDir;
            if (direction == Vector2.zero)
            {
                if (Player.MyInstance._prefabs.transform.localScale.x == -1) // 플레이어가 바라보고 있는 방향확인
                    direction = new Vector2(1, 0);      // localscale이 -1일때 왼쪽을 보고 있음
                else
                    direction = new Vector2(-1, 0);
            }
        }
    }

    private void Start()
    {
        switch (spellType)
        {
            case SpellType.Skill_File_01:
                StartCoroutine(Skill_Fire_01());
                break;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Vector2 direction = MyTarget.position - transform.position; // 타겟과 스펠의 방향과 크기 구함
        myRigidbody.velocity = direction.normalized * speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용

        // Math.Atan2 탄젠트 값으로 각도를 산출 https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg 라디안 각도 변환해주는 상수 http://jw910911.tistory.com/6
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 축 중심 각도로 회전
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox"))// && collision.transform.position == MyTarget.position 원래코드 삭제 (유도기능 넣을때 추가하면 좋을듯)
        {
            Character c = collision.GetComponentInParent<Character>();
            if (!CheckHitEnemy(collision))
            {
                c.TakeDamage(damage, direction); // 피격된 대상에게 자신의 위치 정보 전달
                Instantiate(puff, transform.position, Quaternion.identity);
                //myRigidbody.velocity = Vector3.zero;
                //MyTarget = null;
            }
        }
    }

    private bool CheckHitEnemy(Collider2D collision) // 스킬 한번 맞았으면 다시 안맞게 체크
    {
        GameObject g = collision.transform.parent.gameObject;

        if (!hitEnemy.Contains(g))
        {
            hitEnemy.Add(g);
            return false;
        }
        else
            return true;

    }

    private IEnumerator Skill_Fire_01()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
    //private void Misile()
    //{
    //    Vector2 direction = MyTarget.position - transform.position; // 타겟과 스펠의 방향과 크기 구함
    //    myRigidbody.velocity = direction.normalized * speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    // Math.Atan2 탄젠트 값으로 각도를 산출 https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
    //    // Mathf.Rad2Deg 라디안 각도 변환해주는 상수 http://jw910911.tistory.com/6

    //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 축 중심 각도로 회전
    //}
}