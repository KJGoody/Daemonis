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
    public EnemyAttackType enemyAttackType;
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    public Transform MyTarget { get; set; }
    private Vector3 direction;

    private bool IsAOEAttack = false;
    [SerializeField]
    private float WaitWarningSceonds;
    private Transform AOEexitPoint;              // 장판오브젝트 소환 포인트
    public int AOEDamage;                       // 장판오브젝트 데미지
    [SerializeField]
    private int AEtimes;                        // 장판의 공격 횟수
    private float LastAttackTime = 1000f;
    public float AEwaitforseconds = 0f;             // 장판 공격간 쉬는 시간
    private bool IsPlayerInArea;
    private bool IsAOEAttacking;
    private bool IsAOEAttackingNow = false;


    Coroutine PlayCoroutine;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        MyTarget = GameObject.Find("HitBox_Player").GetComponent<Transform>();
        direction = MyTarget.position - transform.position;
        AOEexitPoint = this.GetComponent<Transform>();
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

    private void Update()
    {
        if (IsAOEAttacking && !IsAOEAttackingNow && IsPlayerInArea && LastAttackTime >= AEwaitforseconds)
        {
            IsAOEAttackingNow = true;
            PlayCoroutine = StartCoroutine(AETimes());
        }
        else if (IsAOEAttackingNow && !IsPlayerInArea)
        {
            IsAOEAttackingNow = false;
            StopCoroutine(PlayCoroutine);
        }
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = direction.normalized * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        LastAttackTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("HitBox_Player"))
        {
            IsPlayerInArea = true;

            if (!IsAOEAttack)
            {
                Character c = collision.GetComponentInParent<Character>();
                string TextType = "PlayerDamage";
                c.TakeDamage(damage, direction, null, TextType);
                speed = 0;
                myRigidbody.velocity = Vector3.zero;
                MyTarget = null;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox_Player"))
            IsPlayerInArea = false;
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
        WarningArea warningarea = Instantiate(Resources.Load("EnemyAttack/WaringArea") as GameObject, this.transform.position, Quaternion.identity).GetComponent<WarningArea>();
        warningarea.destroyTime = WaitWarningSceonds;
        yield return new WaitForSeconds(WaitWarningSceonds + 0.5f);
        this.GetComponent<SpriteRenderer>().enabled = true;

        IsAOEAttacking = true;
        yield return new WaitForSeconds(AEwaitforseconds * AEtimes);
        Destroy(gameObject);
    }

    IEnumerator AETimes()
    {
        while (IsAOEAttacking)
        {
            EnemyAttack AEattack = Instantiate(Resources.Load("EnemyAttack/BaseAE_Attack") as GameObject, AOEexitPoint.position, Quaternion.identity).GetComponent<EnemyAttack>();
            AEattack.damage = AOEDamage;
            LastAttackTime = 0f;
            yield return new WaitForSeconds(AEwaitforseconds);
        }
    }
    IEnumerator BaseAEAttack()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}