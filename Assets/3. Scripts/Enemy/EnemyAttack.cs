using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum EnemyAttackType
    {
        MeleeAttack1,
        RangedAttack1,
        RushAttack1
    }
    public EnemyAttackType enemyAttackType;

    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;
    public float Speed
    {
        get
        {
            return speed;
        }
    }
    [SerializeField]
    private int damage;
    public Transform MyTarget { get; set; }
    private Transform source;
    private Vector3 direction;



    private void Start()
    {
        MyTarget = GameObject.Find("HitBox_Player").GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
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
        if (collision.CompareTag("HitBox_Player"))
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage, source, direction);
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
            DestroyObject(gameObject);
        }
    }

    private IEnumerator MeleeAttack1()
    {
        yield return new WaitForSeconds(0.15f);     // 객체 생존시간
        DestroyObject(gameObject);
    }

    private IEnumerator RangedAttack1()
    {
        yield return new WaitForSeconds(10);
        DestroyObject(gameObject);
    }

    private IEnumerator RushAttack1()
    {
        yield return new WaitForSeconds(100);
        DestroyObject(gameObject);
    }
}