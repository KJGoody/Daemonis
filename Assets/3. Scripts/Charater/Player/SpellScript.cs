using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Skill_Fire_08_Object,
        Skill_Fire_09,
        Skill_Fire_10,
        Skill_Fire_11,
        Skill_Fire_11_Object,
        Skill_Fire_12,
        Skill_Fire_12_Object,
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
                    return "Skill_Fire_07";

                case SpellNames.Skill_Fire_08:
                    return "Skill_Fire_08";

                case SpellNames.Skill_Fire_08_Object:
                    return "Skill_Fire_01";

                case SpellNames.Skill_Fire_09:
                    return "Skill_Fire_09";

                case SpellNames.Skill_Fire_10:
                    return "Skill_Fire_10";

                case SpellNames.Skill_Fire_11:
                    return "Skill_Fire_11";

                case SpellNames.Skill_Fire_11_Object:
                    return "Skill_Fire_11_Object";

                case SpellNames.Skill_Fire_12:
                    return "Skill_Fire_12";

                case SpellNames.Skill_Fire_12_Object:
                    return "Skill_Fire_12_Object";

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

    private Coroutine CurrentCoroutine;

    private List<GameObject> HitEnemy = new List<GameObject>();

    private void Start()
    {
        // �����ٵ� ����
        if (SpellType == SpellInfo.SpellType.Launch)
            myRigidbody = GetComponent<Rigidbody2D>();
        else if (SpellType == SpellInfo.SpellType.Toggle || SpellType == SpellInfo.SpellType.Target)  // ��� ������ �����ٵ� ����
            transform.position += new Vector3(0, 0.3f);  // ��� �߽��� ��ġ ����

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

        // ���� ���̵�� �⺻ ������ �޾ƿ´�.
        SpellType = DataTableManager.Instance.GetInfo_Spell(GetName).Type;
        Speed = DataTableManager.Instance.GetInfo_Spell(GetName).Speed;
        SpellxDamage = DataTableManager.Instance.GetInfo_Spell(GetName).SpellxDamage;

        switch (Name)
        {
            // ȭ����
            case SpellNames.Skill_Fire_01:
                StartCoroutine(Skill_Fire_01());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // ������� 
            case SpellNames.Skill_Fire_03:
                StartCoroutine(Skill_Fire_03());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // �Ǵн�
            case SpellNames.Skill_Fire_04:
                StartCoroutine(Skill_Fire_04());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // ȭ�� ����
            case SpellNames.Skill_Fire_05:
                StartCoroutine(Skill_Fire_05());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // ���� ����
            case SpellNames.Skill_Fire_06:
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // ��ȭ
            case SpellNames.Skill_Fire_07:
                Skill_Fire_07();
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound, 0.2f);
                break;

            // ���ⱸ
            case SpellNames.Skill_Fire_08:
                StartCoroutine(Skill_Fire_08());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // ���ⱸ ������Ʈ
            case SpellNames.Skill_Fire_08_Object:
                StartCoroutine(Skill_Fire_01());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound, 0.125f);
                break;

            // ȭ�� ����̵�
            case SpellNames.Skill_Fire_09:
                StartCoroutine(Skill_Fire_09());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // ȭ�� ����
            case SpellNames.Skill_Fire_10:
                StartCoroutine(Skill_Fire_10());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // �ڵ� ȭ��
            case SpellNames.Skill_Fire_11:
                StartCoroutine(Skill_Fire_11());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // ȭ��
            case SpellNames.Skill_Fire_11_Object:
                StartCoroutine(Skill_Fire_11_Object());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // � �浹
            case SpellNames.Skill_Fire_12:
                StartCoroutine(Skill_Fire_12());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // ������ â
            case SpellNames.Skill_Fire_13:
                StartCoroutine(Skill_Fire_13());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (SpellType != SpellInfo.SpellType.Target)
        {
            if (Name.Equals(SpellNames.Skill_Fire_09))
            {
                if (MyTarget == null)
                    if (GameObject.FindWithTag("Enemy") != null)
                    {
                        if (FindNearestObject() != null)
                            MyTarget = FindNearestObject();
                        else
                            MyTarget = null;
                    }

                if (MyTarget != null)
                    if (!MyTarget.parent.gameObject.GetComponent<EnemyBase>().IsAlive || !MyTarget.parent.gameObject.activeSelf)
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
            else if (SpellType == SpellInfo.SpellType.Launch)
            {
                myRigidbody.velocity = Direction.normalized * Speed; // direction�� normalized�Ͽ� ���Ⱚ���� �ٲ��ְ� �߻��ϴ� �� ����

                if (Name != SpellNames.Skill_Fire_12_Object)
                {
                    ChangeAngle();
                }
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
                if (CurrentCoroutine == null)      // ����̵��� ���� �� �ڷ�ƾ ����
                    CurrentCoroutine = StartCoroutine(TickDamage(0.5f, 0.3f));
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

    private void ChangeAngle()
    {
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;  // Math.Atan2 ź��Ʈ ������ ������ ���� https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
                                                                              // Mathf.Rad2Deg ���� ���� ��ȯ���ִ� ��� http://jw910911.tistory.com/6
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);  // �� �߽� ������ ȸ��
        if (transform.localScale.y >= 0)
        {
            if (angle >= 90 && angle <= 180)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            else if (angle >= -180 && angle <= -90)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }

    private void SpendDamage(Collider2D collision)
    {
        Character character = collision.GetComponentInParent<Character>();

        float PureDamage;
        if (Name == SpellNames.Skill_Fire_07)
            PureDamage = (Player.MyInstance.MyStat.BaseAttack * SpellxDamage) * Player.MyInstance.BuffxDamage * StackxDamage;
        else
            PureDamage = (Player.MyInstance.MyStat.BaseAttack * SpellxDamage) * Player.MyInstance.BuffxDamage;


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
        StartCoroutine(TickDamage(2.5f, 0.5f));
        yield return new WaitForSeconds(5);
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

    private void Skill_Fire_07()
    {
        SpendDamage(MyTarget.GetComponent<Collider2D>());
        Player.MyInstance.RecoverOnHit();
    }

    private IEnumerator Skill_Fire_08()
    {
        StartCoroutine(Skill_Fire_08_Object());
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_08_Object()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            SpellScript spell_1 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_1.Direction = new Vector2(Direction.x, Direction.y);  // ������
            SpellScript spell_2 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_2.Direction = new Vector2(Direction.y, -Direction.x);  // ����
            SpellScript spell_3 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_3.Direction = new Vector2(-Direction.x, -Direction.y);  // �ϴ�
            SpellScript spell_4 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_4.Direction = new Vector2(-Direction.y, Direction.x);  // ����

            SpellScript spell_5 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_5.Direction = new Vector2(Direction.x + Direction.y, Direction.y - Direction.x);  // ���� ���
            SpellScript spell_6 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_6.Direction = new Vector2(Direction.y - Direction.x, -Direction.x - Direction.y);  // ���� �ϴ�
            SpellScript spell_7 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_7.Direction = new Vector2(-Direction.x - Direction.y, -Direction.y + Direction.x);  // ���� �ϴ�
            SpellScript spell_8 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_8.Direction = new Vector2(-Direction.y + Direction.x, Direction.x + Direction.y);  // ���� ���
        }
    }

    private IEnumerator Skill_Fire_09()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_10()
    {
        yield return new WaitForSeconds(0.04f);
        StartCoroutine(TickDamage(2, 10f));
    }

    private IEnumerator Skill_Fire_11()
    {
        ChangeAngle();
        StartCoroutine(Skill_Fire_11_AttackSystem());
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_11_AttackSystem()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            //-- Ȱ ȸ�� �� ���� ���� --
            // ���� Ÿ����
            if (MyTarget == null)
            {
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    if (FindNearestObject() != null)
                        MyTarget = FindNearestObject();
                    else
                        MyTarget = null;
                }
            }

            if (MyTarget != null)
                if (!MyTarget.parent.gameObject.GetComponent<EnemyBase>().IsAlive)
                    MyTarget = null;

            if (MyTarget != null)
                Direction = MyTarget.transform.position - transform.position;

            ChangeAngle();
            GetComponent<Animator>().SetTrigger("Attack");
            yield return new WaitForSeconds(0.01f);

            // ȭ�� �߻�
            SpellScript spell = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_11_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell.Direction = Direction;
            if (MyTarget != null)
                spell.MyTarget = MyTarget;

        }
    }

    private IEnumerator Skill_Fire_11_Object()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_12()
    {
        SpellScript spell = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_12_Object"),
            new Vector2(transform.position.x - 5, transform.position.y + 5),
            Quaternion.identity).GetComponent<SpellScript>();
        spell.Direction = new Vector2(-spell.transform.position.x + transform.position.x, -spell.transform.position.y + transform.position.y);
        yield return new WaitForSeconds(0.85f);
        Destroy(spell.gameObject);
        GetComponent<Animator>().SetTrigger("Start");
        StartCoroutine(TickDamage(2f, 10f));
    }

    private IEnumerator Skill_Fire_13()
    {
        yield return new WaitForSeconds(0.03f);
        StartCoroutine(TickDamage(2f, 10f));
    }

    private Transform FindNearestObject()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 7, LayerMask.GetMask("EnemyHitBox"));

        List<Transform> objects = new List<Transform>();
        for (int i = 0; i < collisions.Length; i++)
            if (collisions[i].transform.parent.gameObject.activeSelf)
                objects.Add(collisions[i].gameObject.transform);

        if (objects.Count == 0)     // ���� ����Ʈ�� ����ٸ� null�� ��ȯ
            return null;

        var neareastObject = objects        // �Ÿ��� ���� ª�� ������Ʈ�� ���Ѵ�.
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject.transform;
    }

    private IEnumerator TickDamage(float Radius, float WaitSconds)
    {
        while (true)
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, Radius, LayerMask.GetMask("EnemyHitBox"));
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
            yield return new WaitForSeconds(WaitSconds);
        }
    }
}