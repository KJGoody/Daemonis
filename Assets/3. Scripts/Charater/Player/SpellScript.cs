using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    public enum SpellNames
    {
        #region ��ų �̸�
        Skill_Fire_01,
        Skill_Fire_03,
        Skill_Fire_04,
        Skill_Fire_05,
        Skill_Fire_06,
        Skill_Fire_07,
        Skill_Fire_08,
        Skill_Fire_09,
        Skill_Fire_10,
        Skill_Fire_11,
        Skill_Fire_12,
        Skill_Fire_13 
        #endregion
    }
    [SerializeField] private SpellNames Name;
    private string GetName
    {
        get
        {
            #region ��ų �̸� ��ȯ
            switch (Name)
            {
                case SpellNames.Skill_Fire_01:
                    return "Skill_Fire_01";

                case SpellNames.Skill_Fire_03:
                    return "Skill_Fire_03";

                case SpellNames.Skill_Fire_04:
                    return "Skill_Fire_04";

                case SpellNames.Skill_Fire_05:
                    return "Skill_Fire_05";

                case SpellNames.Skill_Fire_06:
                    return "Skill_Fire_06";

                case SpellNames.Skill_Fire_07:
                    return "Skill_Fire_0";

                case SpellNames.Skill_Fire_08:
                    return "Skill_Fire_08";

                case SpellNames.Skill_Fire_09:
                    return "Skill_Fire_09";

                case SpellNames.Skill_Fire_10:
                    return "Skill_Fire_10";

                case SpellNames.Skill_Fire_11:
                    return "Skill_Fire_11";

                case SpellNames.Skill_Fire_12:
                    return "Skill_Fire_12";

                case SpellNames.Skill_Fire_13:
                    return "Skill_Fire_13";

                default:
                    return null;
            }
            #endregion
        }
    }
    private SpellInfo.SpellType SpellType;

    [HideInInspector] public Transform MyTarget;  // ���� ���
    [HideInInspector] public Vector2 Direction;  // ���� ����

    private float Speed;  // ���� �ӵ�
    private float SpellxDamage;  // ��ų ������ ���
    [HideInInspector] public float StackxDamage;  // ���� ���

    private Coroutine TickCoroutine;

    private List<GameObject> HitEnemy = new List<GameObject>();

    private void Start()
    {
        // ���� ���̵�� �⺻ ������ �޾ƿ´�.
        SpellType = DataTableManager.Instance.GetInfo_Spell(GetName).Type;
        SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
        Speed = DataTableManager.Instance.GetInfo_Spell(GetName).Speed;
        SpellxDamage = DataTableManager.Instance.GetInfo_Spell(GetName).SpellxDamage;

        switch (Name)
        {
            // ȭ����
            case SpellNames.Skill_Fire_01:
                StartCoroutine(Skill_Fire_01());
                break;

            case SpellNames.Skill_Fire_03:   // ��� ����
                StartCoroutine(Skill_Fire_03());
                break;

            // �Ǵн�
            case SpellNames.Skill_Fire_04:
                StartCoroutine(Skill_Fire_04());
                break;

            // ȭ�� ����
            case SpellNames.Skill_Fire_05:
                StartCoroutine(Skill_Fire_05());
                break;

            // ���� ����
            case SpellNames.Skill_Fire_06:
                break;

            case SpellNames.Skill_Fire_07:   // ��ȭ
                StartCoroutine(Skill_Fire_07());
                break;

            case SpellNames.Skill_Fire_08:
                StartCoroutine(Skill_Fire_08());
                break;

            case SpellNames.Skill_Fire_09:   // ȭ�� ����̵�
                StartCoroutine(Skill_Fire_09());
                break;
        }

        if (SpellType == SpellInfo.SpellType.Toggle || SpellType == SpellInfo.SpellType.Target)  // ��� ������ �����ٵ� ����
            transform.position += new Vector3(0, 0.3f);  // ��� �߽��� ��ġ ����
        else
            myRigidbody = GetComponent<Rigidbody2D>();

        // ���� ���� ����
        if (MyTarget != null)
            Direction = MyTarget.position - transform.position;
        else
        {
            if (Direction == Vector2.zero)
            {
                if (Player.MyInstance._prefabs.transform.localScale.x == -1)  // �÷��̾ �ٶ󺸰� �ִ� ����Ȯ��
                    Direction = new Vector2(1, 0);  // localscale�� -1�϶� ������ ���� ����
                else
                    Direction = new Vector2(-1, 0);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (SpellType != SpellInfo.SpellType.Target)
        {
            if (MyTarget == null && Name == SpellNames.Skill_Fire_09)    // ����̵� ���� �� ����Ÿ����
            {
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    if (FindNearestObject() != null)
                        MyTarget = FindNearestObject().transform.Find("HitBox").transform;
                    else
                        MyTarget = null;
                }
            }

            if (Name.Equals(SpellNames.Skill_Fire_09))
            {
                if (MyTarget != null)
                    if (!MyTarget.parent.gameObject.GetComponent<EnemyBase>().IsAlive)
                        MyTarget = null;

                if (MyTarget != null)
                    Direction = MyTarget.position - transform.position; // Ÿ�ٰ� ������ ����� ũ�� ����
                myRigidbody.velocity = Direction.normalized * Speed;    // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����
            }
            else if (SpellType == SpellInfo.SpellType.Toggle)
            {
                switch (Name)
                {
                    case SpellNames.Skill_Fire_05:
                        transform.Rotate(new Vector3(0, 0, Time.deltaTime * -300));
                        break;
                }
            }
            else
            {
                myRigidbody.velocity = Direction.normalized * Speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����
                                                                     // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
                                                                     // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6
                float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // �� �߽� ������ ȸ��
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") &&  // �� �浹 �� �ı�
            Name != SpellNames.Skill_Fire_04 &&  // �Ǵн� ����
            Name != SpellNames.Skill_Fire_05 &&  // ȭ�� ���� ����
            Name != SpellNames.Skill_Fire_09  // ȭ�� ����̵� ����
            )
            Destroy(gameObject);

        if (collision.CompareTag("Enemy") && SpellType != SpellInfo.SpellType.AOE)
        {
            if (Name.Equals(SpellNames.Skill_Fire_09))
            {
                if (TickCoroutine == null)      // ����̵��� ���� �� �ڷ�ƾ ����
                    TickCoroutine = StartCoroutine(TickDamage());
            }
            else
            {
                if (!CheckHitEnemy(collision))
                {
                    SpendDamage(collision);
                    Player.MyInstance.RecoverOnHit();

                    if (Player.MyInstance.IsOnBuff("Skill_Fire_02"))       // ��ȭ ���� �� ����� ����
                        collision.transform.parent.GetComponent<EnemyBase>().NewBuff("Debuff_Skill_Fire_02");

                    if (SpellType == SpellInfo.SpellType.Toggle)
                    {
                        switch (Name)
                        {
                            case SpellNames.Skill_Fire_05:
                                PuffPool.Instance.GetObject(PuffPool.PuffPrefabsName.Hit_01).PositioningPuff(collision.transform.position);
                                break;
                        }
                    }
                    else
                        PuffPool.Instance.GetObject(PuffPool.PuffPrefabsName.Hit_01).PositioningPuff(transform.position);
                }
            }
        }
    }

    private void SpendDamage(Collider2D collision)
    {
        Character character = collision.GetComponentInParent<Character>();

        float PureDamage = (Player.MyInstance.MyStat.BaseAttack * SpellxDamage) * Player.MyInstance.BuffxDamage;

        if (ChanceMaker.GetThisChanceResult_Percentage(Player.MyInstance.MyStat.CriticalPercent))
            character.TakeDamage(Character.DamageType.Masic, Player.MyInstance.MyStat.HitPercent, PureDamage, Player.MyInstance.MyStat.Level, Direction, NewTextPool.NewTextPrefabsName.Critical);
        else
            character.TakeDamage(Character.DamageType.Masic, Player.MyInstance.MyStat.HitPercent, PureDamage, Player.MyInstance.MyStat.Level, Direction, NewTextPool.NewTextPrefabsName.Enemy);
    }

    private bool CheckHitEnemy(Collider2D collision) // ��ų �ѹ� �¾����� �ٽ� �ȸ°� üũ
    {
        GameObject Target = collision.transform.parent.gameObject;

        if (!HitEnemy.Contains(Target))
        {
            HitEnemy.Add(Target);
            StartCoroutine(ChangeNoHitEnemy(Target));
            return false;
        }
        else
            return true;
    }

    private IEnumerator ChangeNoHitEnemy(GameObject Target)
    {
        yield return new WaitForSeconds(0.1f);
        HitEnemy.Remove(Target);
    }

    public void Event_EndAnimation()
    {
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_01()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_03()
    {
        float AOERadius = 0.5f;             // ���� ����
        int AOETimes = 5;                   // ���� �ǰ� Ƚ��
        float AOEWaitForSeconds = 0.5f;     // ���� �ǰ� �ð�

        for (int i = 0; i < AOETimes; i++)
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, AOERadius, LayerMask.GetMask("HitBox"));
            if (collisions != null)
            {
                for (int j = 0; j < collisions.Length; j++)
                    if (collisions[j].CompareTag("Enemy"))
                    {
                        if (Player.MyInstance.IsOnBuff("Skill_Fire_02"))
                            collisions[j].transform.parent.GetComponent<EnemyBase>().NewBuff("Debuff_Skill_Fire_02");
                        SpendDamage(collisions[j]);
                        Player.MyInstance.RecoverOnHit();
                        PuffPool.Instance.GetObject(PuffPool.PuffPrefabsName.Hit_01).PositioningPuff(collisions[j].transform.position);
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

    private IEnumerator Skill_Fire_07()
    {
        SpendDamage(MyTarget.GetComponent<Collider2D>());
        Player.MyInstance.RecoverOnHit();
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_08()
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

        while (true)
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, Radius, LayerMask.GetMask("HitBox"));
            if (collisions != null)
            {
                for (int j = 0; j < collisions.Length; j++)
                    if (collisions[j].CompareTag("Enemy"))
                    {
                        if (Player.MyInstance.IsOnBuff("Skill_Fire_02"))
                            collisions[j].transform.parent.GetComponent<EnemyBase>().NewBuff("Debuff_Skill_Fire_02");
                        SpendDamage(collisions[j]);
                        Player.MyInstance.RecoverOnHit();
                        PuffPool.Instance.GetObject(PuffPool.PuffPrefabsName.Hit_01).PositioningPuff(collisions[j].transform.position);
                    }
            }
            yield return new WaitForSeconds(WaitForSconds);
        }
    }
}