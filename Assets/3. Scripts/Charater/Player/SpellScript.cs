using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private GameObject puff;

    public enum SpellType
    {
        Skill_File_01,
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
    private bool IsAOEAttack = false;
    private bool IsToggleAttack = false;

    [HideInInspector]
    public Transform MyTarget;    // 공격할 대상
    private Vector2 direction;
    [HideInInspector]
    public Vector2 atkDir;

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    public float CastTime;

    private Coroutine TickCoroutine;

    private List<GameObject> hitEnemy = new List<GameObject>();

    private void Start()
    {
        switch (spellType)
        {
            case SpellType.Skill_File_01:   // 화염구
                StartCoroutine(Skill_Fire_01());
                break;

            case SpellType.Skill_File_03:   // 용암 지대
                IsAOEAttack = true;
                StartCoroutine(Skill_Fire_03());
                break;

            case SpellType.Skill_File_04:   // 피닉스
                StartCoroutine(Skill_Fire_04());
                break;

            case SpellType.Skill_File_05:   // 화염 위성
                IsToggleAttack = true;
                StartCoroutine(Skill_Fire_05());
                break;

            case SpellType.Skill_File_09:   // 화염 토네이도
                StartCoroutine(Skill_Fire_09());
                break;
        }

        if (!IsToggleAttack)    // 토글 공격은 리기드바디가 없음
            myRigidbody = GetComponent<Rigidbody2D>();
        else
            transform.position += new Vector3(0, 0.3f); // 토글 중심점 위치 변경

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

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (MyTarget == null && spellType == SpellType.Skill_File_09)    // 토네이도 공격 시 오토타겟팅
        {
            if (GameObject.FindWithTag("Enemy") != null)
            {
                if (FindNearestObject() != null)
                    MyTarget = FindNearestObject().transform.Find("HitBox").transform;
                else
                    MyTarget = null;
            }
        }

        if (spellType == SpellType.Skill_File_09)
        {
            if(MyTarget != null)
                direction = MyTarget.position - transform.position; // 타겟과 스펠의 방향과 크기 구함
            myRigidbody.velocity = direction.normalized * speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용
        }
        else if (!IsToggleAttack)
        {
            myRigidbody.velocity = direction.normalized * speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용
                                                                 // Math.Atan2 탄젠트 값으로 각도를 산출 https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
                                                                 // Mathf.Rad2Deg 라디안 각도 변환해주는 상수 http://jw910911.tistory.com/6
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 축 중심 각도로 회전
        }
        else
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * -300));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && !IsToggleAttack)
            Destroy(gameObject);

        if (collision.CompareTag("Enemy") && !IsAOEAttack)
        {
            if (!CheckHitEnemy(collision) && spellType != SpellType.Skill_File_09)
            {
                SpendDamage(collision, damage);
                if (!IsToggleAttack)
                    Instantiate(puff, transform.position, Quaternion.identity);
                else
                    Instantiate(puff, collision.transform.position, Quaternion.identity);
            }

            if (spellType == SpellType.Skill_File_09 && TickCoroutine == null)
                TickCoroutine = StartCoroutine(TickDamage());
        }
    }

    private void SpendDamage(Collider2D collision, int damage)
    {
        Character character = collision.GetComponentInParent<Character>();

        string TextType = "EnemyDamage";                                   // 텍스트 타입 설정
        character.TakeDamage(damage, direction, TextType);            // 데미지 전송
    }

    private bool CheckHitEnemy(Collider2D collision) // 스킬 한번 맞았으면 다시 안맞게 체크
    {
        GameObject g = collision.transform.parent.gameObject;

        if (!hitEnemy.Contains(g))
        {
            hitEnemy.Add(g);
            StartCoroutine(ChangeNoHitEnemy(g));
            return false;
        }
        else
            return true;
    }

    private IEnumerator ChangeNoHitEnemy(GameObject Object)
    {
        yield return new WaitForSeconds(0.1f);
        hitEnemy.Remove(Object);
    }

    private IEnumerator Skill_Fire_01()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_03()
    {
        float AOERadius = 0.5f;     // 장판 범위
        int AOETimes = 5;       // 장판 피격 횟수
        float AOEWaitForSeconds = 0.5f;     // 다음 피격 시간
        int AOEDamage = 5;      // 장판 데미지

        for (int i = 0; i < AOETimes; i++)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, AOERadius, LayerMask.GetMask("HitBox"));
            if (collider != null)
            {
                for (int j = 0; j < collider.Length; j++)
                    if (collider[j].CompareTag("Enemy"))
                    {
                        SpendDamage(collider[j], AOEDamage);
                        Instantiate(puff, collider[j].transform.position, Quaternion.identity);
                    }
            }

            yield return new WaitForSeconds(AOEWaitForSeconds);
        }
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_04()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_05()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_09()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private GameObject FindNearestObject()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 7, LayerMask.GetMask("HitBox"));

        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < collisions.Length; i++)
            if (collisions[i].CompareTag("Enemy"))       // 테그가 적인것을 찾는다.
                objects.Add(collisions[i].gameObject);

        if (objects.Count == 0)     // 만약 리스트가 비었다면 null을 반환
            return null;

        var neareastObject = objects        // 거리가 가장 짧은 오브젝트를 구한다.
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject.transform.parent.gameObject;
    }

    private IEnumerator TickDamage()
    {
        float Radius = 0.5f;
        float WaitForSconds = 0.3f;
        int TickDamage = 2;

        while (true)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, Radius, LayerMask.GetMask("HitBox"));
            if (collider != null)
            {
                for (int j = 0; j < collider.Length; j++)
                    if (collider[j].CompareTag("Enemy"))
                    {
                        SpendDamage(collider[j], TickDamage);
                        Instantiate(puff, collider[j].transform.position, Quaternion.identity);
                    }
            }
            yield return new WaitForSeconds(WaitForSconds);
        }
    }
}