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
        BaseAEAttack,                         // ���� ������ ������ ������
        Kobold_Melee_Attack,
        Kobold_Ranged_Attack
    }
    [SerializeField]
    private EnemyAttackType enemyAttackType;
    private bool IsAOEAttack = false;

    [HideInInspector]
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

            case EnemyAttackType.Kobold_Melee_Attack:
                AttackxDamage = 1;
                StartCoroutine(Kobold_Melee_Attack());
                break;

            case EnemyAttackType.Kobold_Ranged_Attack:
                AttackxDamage = 1;
                StartCoroutine(Kobold_Ranged_Attack());
                break;
        }

    }

    private void FixedUpdate()
    {
        if (enemyAttackType.Equals(EnemyAttackType.Kobold_Ranged_Attack))
        {
            myRigidbody.velocity = direction.normalized * speed;
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * -600));
        }
        else
        {
            myRigidbody.velocity = direction.normalized * speed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
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

        float PureDamage = (parent.MyStat.BaseAttack * AttackxDamage) * parent.BuffxDamage;
        character.TakeDamage(Character.DamageType.Physic, parent.MyStat.HitPercent, PureDamage, parent.MyStat.Level, direction, NewTextPool.NewTextPrefabsName.Player);            // ������ ����
    }

    IEnumerator BaseMeleeAttack()
    {
        yield return new WaitForSeconds(0.15f);     // ��ü �����ð�
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
        float AOERadius = 0.5f;     // ���� ����   
        int AOETimes = 5;           // ���� �ǰ� Ƚ��
        float AOEWaitForSeconds = 0.5f;     // ���� �ǰ� �ð�

            // �������� ǥ��
        WarningArea warningarea = Instantiate(Resources.Load("EnemyAttack/WaringArea") as GameObject, transform).GetComponent<WarningArea>();
        warningarea.destroyTime = 1f;
        yield return new WaitForSeconds(1f + 0.5f);
        this.GetComponent<SpriteRenderer>().enabled = true;

        // AOE ���� ����
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

    private IEnumerator Kobold_Melee_Attack()
    {
        yield return new WaitForSeconds(0.15f);     // ��ü �����ð�
        Destroy(gameObject);
    }

    private IEnumerator Kobold_Ranged_Attack()
    {
        yield return new WaitForSeconds(100000);
        Destroy(gameObject);
    }
}