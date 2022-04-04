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
        BaseAOEtickObj                         // 장판 공격의 실질적 데미지
    }
    public EnemyAttackType enemyAttackType;
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    public Transform MyTarget { get; set; }
    private Transform source;
    private Vector3 direction;
   
    private bool IsAOEAttack = false;
    [SerializeField]                            
    private float WaitWarningSceonds;
    public Transform AOEexitPoint;              // 장판오브젝트 소환 포인트
    public int AOEDamage;                       // 장판오브젝트 데미지
    [SerializeField]                            
    private int AOETickTime;                    // 장판의 공격 횟수
    public float AOETime;                       // 장판 공격간 쉬는 시간




    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        MyTarget = GameObject.Find("HitBox_Player").GetComponent<Transform>();
        direction = MyTarget.position - transform.position;

        switch (enemyAttackType)
        {
            case EnemyAttackType.BaseMeleeAttack:
                StartCoroutine(MeleeAttack1());
                break;

            case EnemyAttackType.BaseRangedAttack:
                StartCoroutine(RangedAttack1());
                break;

            case EnemyAttackType.BaseRushAttack:
                StartCoroutine(RushAttack1());
                break;

            case EnemyAttackType.BaseAOEAttack:
                IsAOEAttack = true;
                this.GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(AOEAttack1());
                break;

            case EnemyAttackType.BaseAOEtickObj:
                StartCoroutine(AOEtickObj());
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
        if (collision.CompareTag("HitBox_Player")&&!IsAOEAttack)
        {
            Character c = collision.GetComponentInParent<Character>();
            c.TakeDamage(damage, source, direction);
            speed = 0;
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
            Destroy(gameObject);
        }
    }

    private IEnumerator MeleeAttack1()
    {
        yield return new WaitForSeconds(0.15f);     // 객체 생존시간
        Destroy(gameObject);
    }

    private IEnumerator RangedAttack1()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private IEnumerator RushAttack1()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private IEnumerator AOEAttack1()
    {
        WarningArea warningarea = Instantiate(Resources.Load("EnemyAttack/WaringArea") as GameObject, AOEexitPoint.position, Quaternion.identity).GetComponent<WarningArea>();
        warningarea.destroyTime = 1f;
        yield return new WaitForSeconds(WaitWarningSceonds + 0.5f);
        this.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(AOEtick());
        yield return new WaitForSeconds(AOETime * AOETickTime);
        Destroy(gameObject);
    }

    private IEnumerator AOEtickObj()
    {
        yield return new WaitForSeconds(AOETime);
        Destroy(gameObject);
    }
    private IEnumerator AOEtick()
    {
        for (int i = 0; i <= AOETickTime; i++)
        {
            yield return new WaitForSeconds(AOETime);
            EnemyAttack AOEObj = Instantiate(Resources.Load("EnemyAttack/BaseAE_Attack") as GameObject, AOEexitPoint.position, Quaternion.identity).GetComponent<EnemyAttack>();
            AOEObj.damage = AOEDamage;
            AOEObj.AOETime = AOETime;
        }
    }
}