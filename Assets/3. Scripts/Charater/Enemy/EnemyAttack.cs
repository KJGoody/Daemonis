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
    private bool IsAOEAttack = false;

    public EnemyBase parent;

    private Transform MyTarget;
    private Vector3 direction;

    private Rigidbody2D myRigidbody;
    [SerializeField]
    private float speed;
    private int AttackxDamage;


    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        MyTarget = GameObject.Find("Player").GetComponent<Transform>();
        direction = MyTarget.position - transform.position;
        
    }

    private void Start()
    {
        switch (enemyAttackType)
        {
            case EnemyAttackType.BaseMeleeAttack:
                AttackxDamage = 1;
                StartCoroutine(BaseMeleeAttack());
                break;

            case EnemyAttackType.BaseRangedAttack:
                AttackxDamage = 1;
                StartCoroutine(BaseRangedAttack());
                break;

            case EnemyAttackType.BaseRushAttack:
                AttackxDamage = 1;
                StartCoroutine(BaseRushAttack());
                break;

            case EnemyAttackType.BaseAOEAttack:
                IsAOEAttack = true;
                this.GetComponent<SpriteRenderer>().enabled = false;
                AttackxDamage = 1;
                StartCoroutine(BaseAOEAttack());
                break;

            case EnemyAttackType.BaseAEAttack:
                AttackxDamage = 1;
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

        if (collision.CompareTag("Player") && !IsAOEAttack)
        {
            SpendDamage(collision);
            speed = 0;
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
            Destroy(gameObject);
        }
    }

    private void SpendDamage(Collider2D collision)
    {
        Character character = collision.GetComponentInParent<Character>();

        float PureDamage = (parent.MyStat.Attak * AttackxDamage) * parent.BuffxDamage;
        character.TakeDamage(true, parent.MyStat.HitPercent, PureDamage, parent.MyStat.Level, direction, "PlayerDamage");            // 데미지 전송
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
        float AOERadius = 0.5f;     // 장판 범위   
        int AOETimes = 5;           // 장판 피격 횟수
        float AOEWaitForSeconds = 0.5f;     // 다음 피격 시간

            // 위험지역 표시
        WarningArea warningarea = Instantiate(Resources.Load("EnemyAttack/WaringArea") as GameObject, transform).GetComponent<WarningArea>();
        warningarea.destroyTime = 1f;
        yield return new WaitForSeconds(1f + 0.5f);
        this.GetComponent<SpriteRenderer>().enabled = true;

        // AOE 공격 시작
        for(int i = 0; i < AOETimes; i++)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, AOERadius, LayerMask.GetMask("Player"));
            if (collider != null)
                SpendDamage(collider);

            yield return new WaitForSeconds(AOEWaitForSeconds);
        }
        Destroy(gameObject);
    }

    IEnumerator BaseAEAttack()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}