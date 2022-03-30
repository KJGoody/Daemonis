using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpellScripts : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;

    public Transform MyTarget { get; set; }
    private Transform source;
    private int damage;
    private Vector2 direction;
    List<GameObject> hitPlayer = new List<GameObject>();

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        direction = MyTarget.position - transform.position;
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Arrow()
    {
        Vector2 direction = MyTarget.position - transform.position;
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
        if (collision.CompareTag("HitBox"))// && collision.transform.position == MyTarget.position �����ڵ� ���� (������� ������ �߰��ϸ� ������)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            if (!CheckHitPlayer(collision))
                c.TakeDamage(damage, source, direction); // �ǰݵ� ��󿡰� �ڽ��� ��ġ ���� ����
            GetComponent<Animator>().SetTrigger("impact");
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
        }
    }
    private bool CheckHitPlayer(Collider2D collision) // ��ų �ѹ� �¾����� �ٽ� �ȸ°� üũ
    {
        GameObject g = collision.GetComponent<GameObject>();
        if (!hitPlayer.Contains(g))
        {
            hitPlayer.Add(g);
            return false;
        }
        else
            return true;

    }
}
