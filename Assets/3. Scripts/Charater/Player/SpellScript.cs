using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private GameObject puff;
    
    public enum SpellType
    {
        Skill_File_01,
        Skill_File_02,
        Skill_File_03,
        Skill_File_04,
        Skill_File_05,
        Skill_File_06,
        Skill_File_07,
        Skill_File_08,
        Skill_File_09
    }
    [SerializeField]
    private SpellType spellType;

    public Transform MyTarget { get; set; } // ������ ���
    private Vector2 direction;
    public Vector2 atkDir;

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;

    private List<GameObject> hitEnemy = new List<GameObject>();

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        if (MyTarget != null)
            direction = MyTarget.position - transform.position;
        else
        {
            direction = atkDir;
            if (direction == Vector2.zero)
            {
                if (Player.MyInstance._prefabs.transform.localScale.x == -1) // �÷��̾ �ٶ󺸰� �ִ� ����Ȯ��
                    direction = new Vector2(1, 0);      // localscale�� -1�϶� ������ ���� ����
                else
                    direction = new Vector2(-1, 0);
            }
        }
    }

    private void Start()
    {
        switch (spellType)
        {
            case SpellType.Skill_File_01:
                StartCoroutine(Skill_Fire_01());
                break;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Vector2 direction = MyTarget.position - transform.position; // Ÿ�ٰ� ������ ����� ũ�� ����
        myRigidbody.velocity = direction.normalized * speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����

        // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
        // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // �� �߽� ������ ȸ��
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox"))// && collision.transform.position == MyTarget.position �����ڵ� ���� (������� ������ �߰��ϸ� ������)
        {
            Character c = collision.GetComponentInParent<Character>();
            if (!CheckHitEnemy(collision))
            {
                c.TakeDamage(damage, direction); // �ǰݵ� ��󿡰� �ڽ��� ��ġ ���� ����
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

    private IEnumerator Skill_Fire_01()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
    //private void Misile()
    //{
    //    Vector2 direction = MyTarget.position - transform.position; // Ÿ�ٰ� ������ ����� ũ�� ����
    //    myRigidbody.velocity = direction.normalized * speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
    //    // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6

    //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // �� �߽� ������ ȸ��
    //}
}