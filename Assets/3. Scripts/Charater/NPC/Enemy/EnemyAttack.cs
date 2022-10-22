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
        BaseAEAttack,                         // 장판 공격의 실질적 데미지
        Kobold_Ranged,
        Dwarf_Ranged,
        Siren_Ranged
    }
    [SerializeField] private EnemyAttackType enemyAttackType;

    [HideInInspector] public EnemyBase parent;

    private Transform MyTarget;
    private Vector3 direction;

    private Rigidbody2D myRigidbody;
    [SerializeField] private float speed;
    private int AttackxDamage;


    private void Awake()
    {
        
        MyTarget = GameObject.Find("Player").GetComponent<Transform>();
        direction = MyTarget.position - transform.position;

    }

    private void Start()
    {
        AttackxDamage = 1;

        switch (enemyAttackType)
        {
            case EnemyAttackType.BaseMeleeAttack:
                myRigidbody = GetComponent<Rigidbody2D>();
                StartCoroutine(BaseMeleeAttack());
                break;

            case EnemyAttackType.BaseRangedAttack:
            case EnemyAttackType.Kobold_Ranged:
            case EnemyAttackType.Dwarf_Ranged:
            case EnemyAttackType.Siren_Ranged:
                myRigidbody = GetComponent<Rigidbody2D>();
                StartCoroutine(BaseRangedAttack());
                break;

            case EnemyAttackType.BaseAOEAttack:
                GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(BaseAOEAttack());
                break;

            case EnemyAttackType.BaseAEAttack:
                StartCoroutine(BaseAEAttack());
                break;

            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (enemyAttackType)
        {
            case EnemyAttackType.BaseRangedAttack:
            case EnemyAttackType.Dwarf_Ranged:
            case EnemyAttackType.Siren_Ranged:
                myRigidbody.velocity = direction.normalized * speed;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;

            case EnemyAttackType.BaseMeleeAttack:
            case EnemyAttackType.Kobold_Ranged:
                myRigidbody.velocity = direction.normalized * speed;
                transform.Rotate(new Vector3(0, 0, Time.deltaTime * -600));
                break;

            case EnemyAttackType.BaseRushAttack:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch (enemyAttackType)
        {
            case EnemyAttackType.BaseRangedAttack:
                if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    Destroy(gameObject);
                break;

            case EnemyAttackType.BaseRushAttack:
                if (!parent.IsRushing)
                    Destroy(gameObject);
                break;

            default:
                break;
        }

        if (collision.CompareTag("Player"))
        {
            SpendDamage(collision);
            Destroy(gameObject);
        }
    }

    private void SpendDamage(Collider2D collision)
    {
        Character character = collision.GetComponentInParent<Character>();

        float PureDamage = (parent.MyStat.BaseAttack * AttackxDamage) * parent.BuffxDamage;
        character.TakeDamage(Character.DamageType.Physic, parent.MyStat.HitPercent, PureDamage, parent.MyStat.Level, direction, NewTextPool.NewTextPrefabsName.Player);            // 데미지 전송
    }

    private IEnumerator BaseMeleeAttack()
    {
        yield return new WaitForSeconds(0.15f);     // 객체 생존시간
        Destroy(gameObject);
    }

    private IEnumerator BaseRangedAttack()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private IEnumerator BaseAOEAttack()
    {
        float AOERadius = 0.5f;     // 장판 범위   
        int AOETimes = 5;           // 장판 피격 횟수
        float AOEWaitForSeconds = 0.5f;     // 다음 피격 시간

        // 위험지역 표시
        yield return StartCoroutine(WarningArea(1));

        // AOE 공격 시작
        for (int i = 0; i < AOETimes; i++)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, AOERadius, LayerMask.GetMask("Player"));
            if (collider != null)
                SpendDamage(collider);

            yield return new WaitForSeconds(AOEWaitForSeconds);
        }
        Destroy(gameObject);
    }

    private IEnumerator BaseAEAttack()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private IEnumerator WarningArea(int destoryseconds)
    {
        WarningArea warningarea = Instantiate(Resources.Load("EnemyAttack/WaringArea") as GameObject, transform).GetComponent<WarningArea>();
        warningarea.destroyTime = destoryseconds;
        yield return new WaitForSeconds(destoryseconds + 0.5f);
        GetComponent<SpriteRenderer>().enabled = true;
    }
}