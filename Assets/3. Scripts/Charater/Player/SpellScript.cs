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
    private bool IsAOEAttack = false;

    public Transform MyTarget { get; set; } // ������ ���
    private Vector2 direction;
    [HideInInspector]
    public Vector2 atkDir;

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    public float CastTime;

    private List<GameObject> hitEnemy = new List<GameObject>();

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
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

        switch (spellType)
        {
            case SpellType.Skill_File_01:
                StartCoroutine(Skill_Fire_01());
                break;

            case SpellType.Skill_File_03:
                IsAOEAttack = true;
                StartCoroutine(Skill_Fire_03());
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            Destroy(gameObject);

        if (collision.CompareTag("Enemy") && !IsAOEAttack)// && collision.transform.position == MyTarget.position �����ڵ� ���� (������� ������ �߰��ϸ� ������)
        {
            Debug.Log(collision);
            if (!CheckHitEnemy(collision))
            {
                SpendDamage(collision, damage);
                Instantiate(puff, transform.position, Quaternion.identity);
            }
        }
    }

    private void SpendDamage(Collider2D collision, int damage)
    {
        Character character = collision.GetComponentInParent<Character>();

        string TextType = "EnemyDamage";                                   // �ؽ�Ʈ Ÿ�� ����
        character.TakeDamage(damage, direction, TextType);            // ������ ����
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

    private IEnumerator Skill_Fire_03()
    {
        float AOERadius = 0.5f;     // ���� ����
        int AOETimes = 5;       // ���� �ǰ� Ƚ��
        float AOEWaitForSeconds = 0.5f;     // ���� �ǰ� �ð�
        int AOEDamage = 5;      // ���� ������

        for (int i = 0; i < AOETimes; i++)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, AOERadius, LayerMask.GetMask("HitBox"));
            if (collider != null)
            {
                for (int j = 0; j < collider.Length; j++)
                    if(collider[j].CompareTag("Enemy"))
                        SpendDamage(collider[j], AOEDamage);
            }

            yield return new WaitForSeconds(AOEWaitForSeconds);
        }
        Destroy(gameObject);
    }
}