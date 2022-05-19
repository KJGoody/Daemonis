using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticleSpell : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private ParticleSystem particleObject;
    [SerializeField]
    private float speed;
    public Transform MyTarget { get; set; } // ������ ���
    private Transform source;
    private int damage;
    private Vector2 direction;
    List<GameObject> hitEnemy = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        particleObject = GetComponent<ParticleSystem>();
        direction = MyTarget.position - transform.position;
    }

    private void FixedUpdate()
    {
        //Vector2 direction = MyTarget.position - transform.position; // Ÿ�ٰ� ������ ����� ũ�� ����
        myRigidbody.velocity = direction.normalized * speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6

        particleObject.startRotation = angle;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // �� �߽� ������ ȸ��

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
            if (!CheckHitEnemy(collision))
                c.TakeDamage(damage, direction, source); // �ǰݵ� ��󿡰� �ڽ��� ��ġ ���� ����
            GetComponent<Animator>().SetTrigger("impact");
            myRigidbody.velocity = Vector3.zero;
            MyTarget = null;
        }
    }
    private bool CheckHitEnemy(Collider2D collision) // ��ų �ѹ� �¾����� �ٽ� �ȸ°� üũ
    {
        GameObject g = collision.GetComponent<GameObject>();
        if (!hitEnemy.Contains(g))
        {
            hitEnemy.Add(g);
            return false;
        }
        else
            return true;

    }

}
