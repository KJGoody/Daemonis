using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private bool IsToggleAttack = false;

    [HideInInspector]
    public Transform MyTarget;    // ������ ���
    private Vector2 direction;
    [HideInInspector]
    public Vector2 atkDir;

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    public float CastTime;

    private Coroutine TickCoroutine;

    private List<GameObject> hitEnemy = new List<GameObject>();

    private void Start()
    {
        switch (spellType)
        {
            case SpellType.Skill_File_01:   // ȭ����
                StartCoroutine(Skill_Fire_01());
                break;

            case SpellType.Skill_File_03:   // ��� ����
                IsAOEAttack = true;
                StartCoroutine(Skill_Fire_03());
                break;

            case SpellType.Skill_File_04:   // �Ǵн�
                StartCoroutine(Skill_Fire_04());
                break;

            case SpellType.Skill_File_05:   // ȭ�� ����
                IsToggleAttack = true;
                StartCoroutine(Skill_Fire_05());
                break;

            case SpellType.Skill_File_09:   // ȭ�� ����̵�
                StartCoroutine(Skill_Fire_09());
                break;
        }

        if (!IsToggleAttack)    // ��� ������ �����ٵ� ����
            myRigidbody = GetComponent<Rigidbody2D>();
        else
            transform.position += new Vector3(0, 0.3f); // ��� �߽��� ��ġ ����

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

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (MyTarget == null && spellType == SpellType.Skill_File_09)    // ����̵� ���� �� ����Ÿ����
        {
            if (GameObject.FindWithTag("Enemy") != null)
            {
                if (FindNearestObject() != null)
                    MyTarget = FindNearestObject().transform.Find("HitBox").transform;
                else
                    MyTarget = null;
            }
        }

        if (spellType == SpellType.Skill_File_09)
        {
            if(MyTarget != null)
                direction = MyTarget.position - transform.position; // Ÿ�ٰ� ������ ����� ũ�� ����
            myRigidbody.velocity = direction.normalized * speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����
        }
        else if (!IsToggleAttack)
        {
            myRigidbody.velocity = direction.normalized * speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����
                                                                 // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
                                                                 // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // �� �߽� ������ ȸ��
        }
        else
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * -300));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && !IsToggleAttack)
            Destroy(gameObject);

        if (collision.CompareTag("Enemy") && !IsAOEAttack)
        {
            if (!CheckHitEnemy(collision) && spellType != SpellType.Skill_File_09)
            {
                SpendDamage(collision, damage);
                if (!IsToggleAttack)
                    Instantiate(puff, transform.position, Quaternion.identity);
                else
                    Instantiate(puff, collision.transform.position, Quaternion.identity);
            }

            if (spellType == SpellType.Skill_File_09 && TickCoroutine == null)
                TickCoroutine = StartCoroutine(TickDamage());
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
            StartCoroutine(ChangeNoHitEnemy(g));
            return false;
        }
        else
            return true;
    }

    private IEnumerator ChangeNoHitEnemy(GameObject Object)
    {
        yield return new WaitForSeconds(0.1f);
        hitEnemy.Remove(Object);
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
                    if (collider[j].CompareTag("Enemy"))
                    {
                        SpendDamage(collider[j], AOEDamage);
                        Instantiate(puff, collider[j].transform.position, Quaternion.identity);
                    }
            }

            yield return new WaitForSeconds(AOEWaitForSeconds);
        }
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_04()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_05()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_09()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private GameObject FindNearestObject()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 7, LayerMask.GetMask("HitBox"));

        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < collisions.Length; i++)
            if (collisions[i].CompareTag("Enemy"))       // �ױװ� ���ΰ��� ã�´�.
                objects.Add(collisions[i].gameObject);

        if (objects.Count == 0)     // ���� ����Ʈ�� ����ٸ� null�� ��ȯ
            return null;

        var neareastObject = objects        // �Ÿ��� ���� ª�� ������Ʈ�� ���Ѵ�.
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject.transform.parent.gameObject;
    }

    private IEnumerator TickDamage()
    {
        float Radius = 0.5f;
        float WaitForSconds = 0.3f;
        int TickDamage = 2;

        while (true)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, Radius, LayerMask.GetMask("HitBox"));
            if (collider != null)
            {
                for (int j = 0; j < collider.Length; j++)
                    if (collider[j].CompareTag("Enemy"))
                    {
                        SpendDamage(collider[j], TickDamage);
                        Instantiate(puff, collider[j].transform.position, Quaternion.identity);
                    }
            }
            yield return new WaitForSeconds(WaitForSconds);
        }
    }
}