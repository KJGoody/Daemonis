using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum EnemyAttackType
    {
        BaseMeleeAttack,
        BaseRangedAttack,
        BaseRushAttack,
        BaseAOEAttack,                         // 장판을 소환하고 일정시간마다 장판오브젝트를 생성해서 플레이어에게 데미지를 준다.
        BaseAEAttack                         // 장판 공격의 실질적 데미지
    }
    [SerializeField]
    private EnemyAttackType enemyAttackType;
    private Transform MyTarget;
    private Vector3 direction;

    private Rigidbody2D myRigidbody;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;

    private bool IsAOEAttack = false;
    private float AoeRadius;                   // 장판 반지름
    [SerializeField]
    private int AoeDamage;                   // 장판오브젝트 데미지
    [SerializeField]
    private int AoeTimes;                     // 장판의 공격 횟수
    [SerializeField]
    private float AoeWaitforSeconds = 0f;    // 장판 공격간 쉬는 시간

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        MyTarget = GameObject.Find("HitBox_Player").GetComponent<Transform>();
        direction = MyTarget.position - transform.position;
        
    }

    private void Start()
    {
        switch (enemyAttackType)
        {
            case EnemyAttackType.BaseMeleeAttack:
                StartCoroutine(BaseMeleeAttack());
                break;

            case EnemyAttackType.BaseRangedAttack:
                StartCoroutine(BaseRangedAttack());
                break;

            case EnemyAttackType.BaseRushAttack:
                StartCoroutine(BaseRushAttack());
                break;

            case EnemyAttackType.BaseAOEAttack:
                IsAOEAttack = true;
                this.GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(BaseAOEAttack());
                break;

            case EnemyAttackType.BaseAEAttack:
                StartCoroutine(BaseAEAttack());
                break;
        }

    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = direction.normalized * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            Destroy(gameObject);

        if (collision.CompareTag("HitBox_Player"))
        {
            if (!IsAOEAttack)
            {
                SpendDamage(collision);

                speed = 0;
                myRigidbody.velocity = Vector3.zero;
                MyTarget = null;
                Destroy(gameObject);
            }
        }
    }

    private void SpendDamage(Collider2D collision)
    {
        Character character = collision.GetComponentInParent<Character>();
        
        string TextType = "PlayerDamage";                                   // 텍스트 타입 설정
        character.TakeDamage(damage, direction, TextType);            // 데미지 전송
    }

    IEnumerator BaseMeleeAttack()
    {
        yield return new WaitForSeconds(0.15f);     // 객체 생존시간
        Destroy(gameObject);
    }

    IEnumerator BaseRangedAttack()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    IEnumerator BaseRushAttack()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    IEnumerator BaseAOEAttack()
    {
        Transform AOEexitPoint = transform;
        float AOERadius = 0.5f;

            // 위험지역 표시
        WarningArea warningarea = Instantiate(Resources.Load("EnemyAttack/WaringArea") as GameObject, this.transform).GetComponent<WarningArea>();
        warningarea.destroyTime = 1f;
        yield return new WaitForSeconds(1f + 0.5f);
        this.GetComponent<SpriteRenderer>().enabled = true;

        // AOE 공격 시작
        for(int i = 0; i < AoeTimes; i++)
        {
            Collider2D collider = Physics2D.OverlapCircle(AOEexitPoint.position, AoeRadius, LayerMask.GetMask("Player"));
            if (collider != null)
                SpendDamage(collider);

            yield return new WaitForSeconds(AoeWaitforSeconds);
        }
        Destroy(gameObject);
    }

    IEnumerator BaseAEAttack()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}