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
        BaseAOEAttack,                         // ������ ��ȯ�ϰ� �����ð����� ���ǿ�����Ʈ�� �����ؼ� �÷��̾�� �������� �ش�.
        BaseAEAttack                         // ���� ������ ������ ������
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
    private Transform AOEexitPoint;              // ���ǿ�����Ʈ ��ȯ ����Ʈ
    public int AOEDamage;                       // ���ǿ�����Ʈ ������
    [SerializeField]                            
    private int AEtimes;                        // ������ ���� Ƚ��
    public float AEwaitforseconds;             // ���� ���ݰ� ���� �ð�




    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        MyTarget = GameObject.Find("HitBox_Player").GetComponent<Transform>();
        direction = MyTarget.position - transform.position;
        AOEexitPoint = this.GetComponent<Transform>();

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
        if (collision.CompareTag("HitBox_Player") && !IsAOEAttack)
        {
            Character c = collision.GetComponentInParent<Character>();
            c.TakeDamage(damage, source, direction);
            speed = 0;
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
            Destroy(gameObject);
        }
    }

    private IEnumerator BaseMeleeAttack()
    {
        yield return new WaitForSeconds(0.15f);     // ��ü �����ð�
        Destroy(gameObject);
    }

    private IEnumerator BaseRangedAttack()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private IEnumerator BaseRushAttack()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private IEnumerator BaseAOEAttack()
    {
        WarningArea warningarea = Instantiate(Resources.Load("EnemyAttack/WaringArea") as GameObject, this.transform.position, Quaternion.identity).GetComponent<WarningArea>();
        warningarea.destroyTime = WaitWarningSceonds;
        yield return new WaitForSeconds(WaitWarningSceonds + 0.5f);
        this.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(AETimes());
        yield return new WaitForSeconds(AEwaitforseconds * AEtimes);
        Destroy(gameObject);
    }

    private IEnumerator AETimes()
    {
        for (int i = 0; i < AEtimes; i++)
        {
            EnemyAttack AEattack = Instantiate(Resources.Load("EnemyAttack/BaseAE_Attack") as GameObject, AOEexitPoint.position, Quaternion.identity).GetComponent<EnemyAttack>();
            AEattack.damage = AOEDamage;
            AEattack.AEwaitforseconds = AEwaitforseconds;
            yield return new WaitForSeconds(AEwaitforseconds);
        }
    }
    private IEnumerator BaseAEAttack()
    {
        yield return new WaitForSeconds(AEwaitforseconds);
        Destroy(gameObject);
    }
}