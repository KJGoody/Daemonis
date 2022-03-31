using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum EnemyAttackType
    {
        MeleeAtack1,
        rangedAttack1
    }
    public EnemyAttackType enemyAttackType;

    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;

    public Transform MyTarget { get; set; }
    private Transform source;
    private int damage;
    private Vector3 direction;



    private void Start()
    {
        MyTarget = GameObject.Find("HitBox_Player").GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        Debug.Log(MyTarget.position);
        Debug.Log(transform.position);

        direction = MyTarget.position - transform.position;
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Initailize(Transform target, int damage, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox_Player"))
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
        }
    }


    private void MeleeAttack1()
    {
        Vector2 direction = MyTarget.position - transform.position;
        myRigidbody.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void rangedAttack1()
    {
        Vector2 direction = MyTarget.position - transform.position;
        myRigidbody.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}
