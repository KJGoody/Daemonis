using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum EnemyAttackType
    {
        MeleeAttack1,
        RangedAttack1,
        RushAttack1,
        AOEAttack1,                         // ������ ��ȯ�ϰ� �����ð����� ���ǿ�����Ʈ�� �����ؼ� �÷��̾�� �������� �ش�.
        AOEtickObj1                         // ���� ������ ������ ������
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
    private float WaitWarningSceonds = 1f;
    public Transform AOEexitPoint;              // ���ǿ�����Ʈ ��ȯ ����Ʈ
    public int AOEDamage;                       // ���ǿ�����Ʈ ������
    [SerializeField]                            
    private int AOETickTime;                    // ������ ���� Ƚ��
    public float AOETime;                       // ���� ���ݰ� ���� �ð�




    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        MyTarget = GameObject.Find("HitBox_Player").GetComponent<Transform>();
        direction = MyTarget.position - transform.position;

        switch (enemyAttackType)
        {
            case EnemyAttackType.MeleeAttack1:
                StartCoroutine(MeleeAttack1());
                break;

            case EnemyAttackType.RangedAttack1:
                StartCoroutine(RangedAttack1());
                break;

            case EnemyAttackType.RushAttack1:
                StartCoroutine(RushAttack1());
                break;

            case EnemyAttackType.AOEAttack1:
                IsAOEAttack = true;
                StartCoroutine(AOEAttack1());
                break;

            case EnemyAttackType.AOEtickObj1:
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
        yield return new WaitForSeconds(0.15f);     // ��ü �����ð�
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
        yield return new WaitForSeconds(WaitWarningSceonds);
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
            EnemyAttack AOEObj = Instantiate(Resources.Load("EnemyAttack/AOEtickObj1") as GameObject, AOEexitPoint.position, Quaternion.identity).GetComponent<EnemyAttack>();
            AOEObj.damage = AOEDamage;
            AOEObj.AOETime = AOETime;
        }
    }
}