using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject puff;

    public Transform MyTarget { get; set; } // ������ ���
    private Transform source;
    private int damage;
    private Vector2 direction;
    private Vector2 atkDir;
    List<GameObject> hitEnemy = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        Invoke("DestroySpell", 7);
        myRigidbody = GetComponent<Rigidbody2D>();
        if (MyTarget != null)
            direction = MyTarget.position - transform.position;
        else
        {
            direction = atkDir;
            if (direction.x == 0 && direction.y == 0)
                direction.x = 1;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //Vector2 direction = MyTarget.position - transform.position; // Ÿ�ٰ� ������ ����� ũ�� ����
        myRigidbody.velocity = direction.normalized * speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // �� �߽� ������ ȸ��

    }


    private void Misile()
    {
        Vector2 direction = MyTarget.position - transform.position; // Ÿ�ٰ� ������ ����� ũ�� ����
        myRigidbody.velocity = direction.normalized * speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // �� �߽� ������ ȸ��
    }


    public void Initailize(int damage, Transform source, Vector2 atkDir)
    {
        this.damage = damage;
        this.source = source;
        this.atkDir = atkDir;


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox"))// && collision.transform.position == MyTarget.position �����ڵ� ���� (������� ������ �߰��ϸ� ������)
        {
            Character c = collision.GetComponentInParent<Character>();
            if (!CheckHitEnemy(collision))
            {
                c.TakeDamage(damage, direction, source); // �ǰݵ� ��󿡰� �ڽ��� ��ġ ���� ����
                Instantiate(puff, transform.position, Quaternion.identity);
                //myRigidbody.velocity = Vector3.zero;
                //MyTarget = null;
            }
        }
    }

    private bool CheckHitEnemy(Collider2D collision) // ��ų �ѹ� �¾����� �ٽ� �ȸ°� üũ
    {
        GameObject g = collision.transform.parent.gameObject;

        if (!hitEnemy.Contains(g))
        {
            hitEnemy.Add(g);
            return false;
        }
        else
            return true;

    }
    private void DestroySpell()
    {
        Destroy(gameObject);
    }
}