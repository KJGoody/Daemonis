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
        #region 스킬 이름
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
            #region 스킬 이름 변환
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

    [HideInInspector] public Transform MyTarget;  // 공격 대상
    [HideInInspector] public Vector2 Direction;  // 진행 방향

    private float Speed;  // 진행 속도
    private float SpellxDamage;  // 스킬 데미지 배수
    [HideInInspector] public float StackxDamage;  // 스텍 배수

    private Coroutine CurrentCoroutine;

    private List<GameObject> HitEnemy = new List<GameObject>();

    private void Start()
    {
        // 리기드바디 설정
        if (SpellType == SpellInfo.SpellType.Launch)
            myRigidbody = GetComponent<Rigidbody2D>();
        else if (SpellType == SpellInfo.SpellType.Toggle || SpellType == SpellInfo.SpellType.Target)  // 토글 공격은 리기드바디가 없음
            transform.position += new Vector3(0, 0.3f);  // 토글 중심점 위치 변경

        // 진행 방향 설정
        if (MyTarget != null)
            Direction = MyTarget.position - transform.position;
        else
        {
            if (Direction == Vector2.zero)
            {
                if (Player.MyInstance._prefabs.transform.localScale.x == -1)  // 플레이어가 바라보고 있는 방향확인
                    Direction = new Vector2(1, 0);  // localscale이 -1일때 왼쪽을 보고 있음
                else
                    Direction = new Vector2(-1, 0);
            }
        }

        // 스팰 아이디로 기본 정보를 받아온다.
        SpellType = DataTableManager.Instance.GetInfo_Spell(GetName).Type;
        Speed = DataTableManager.Instance.GetInfo_Spell(GetName).Speed;
        SpellxDamage = DataTableManager.Instance.GetInfo_Spell(GetName).SpellxDamage;

        switch (Name)
        {
            // 화염구
            case SpellNames.Skill_Fire_01:
                StartCoroutine(Skill_Fire_01());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 용암지대 
            case SpellNames.Skill_Fire_03:
                StartCoroutine(Skill_Fire_03());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 피닉스
            case SpellNames.Skill_Fire_04:
                StartCoroutine(Skill_Fire_04());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 화염 위성
            case SpellNames.Skill_Fire_05:
                StartCoroutine(Skill_Fire_05());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 용의 숨결
            case SpellNames.Skill_Fire_06:
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 점화
            case SpellNames.Skill_Fire_07:
                Skill_Fire_07();
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound, 0.2f);
                break;

            // 분출구
            case SpellNames.Skill_Fire_08:
                StartCoroutine(Skill_Fire_08());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 분출구 오브젝트
            case SpellNames.Skill_Fire_08_Object:
                StartCoroutine(Skill_Fire_01());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound, 0.125f);
                break;

            // 화염 토네이도
            case SpellNames.Skill_Fire_09:
                StartCoroutine(Skill_Fire_09());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 화염 폭발
            case SpellNames.Skill_Fire_10:
                StartCoroutine(Skill_Fire_10());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 자동 화살
            case SpellNames.Skill_Fire_11:
                StartCoroutine(Skill_Fire_11());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 화살
            case SpellNames.Skill_Fire_11_Object:
                StartCoroutine(Skill_Fire_11_Object());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 운석 충돌
            case SpellNames.Skill_Fire_12:
                StartCoroutine(Skill_Fire_12());
                SoundManager.Instance.PlaySFXSound(DataTableManager.Instance.GetInfo_Spell(GetName).Sound);
                break;

            // 심판의 창
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
                    Direction = MyTarget.position - transform.position; // 타겟과 스펠의 방향과 크기 구함
                myRigidbody.velocity = Direction.normalized * Speed;    // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용
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
                myRigidbody.velocity = Direction.normalized * Speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용

                if (Name != SpellNames.Skill_Fire_12_Object)
                {
                    ChangeAngle();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") &&  // 벽 충돌 시 파괴
            Name != SpellNames.Skill_Fire_04 &&  // 피닉스 제외
            Name != SpellNames.Skill_Fire_05 &&  // 화염 위성 제외
            Name != SpellNames.Skill_Fire_09  // 화염 토네이도 제외
            )
            Destroy(gameObject);

        if (collision.CompareTag("Enemy") && SpellType != SpellInfo.SpellType.AOE)
        {
            if (Name.Equals(SpellNames.Skill_Fire_09))
            {
                if (CurrentCoroutine == null)      // 토네이도에 접촉 시 코루틴 시작
                    CurrentCoroutine = StartCoroutine(TickDamage(0.5f, 0.3f));
            }
            else
            {
                if (!CheckHitEnemy(collision))
                {
                    SpendDamage(collision);
                    Player.MyInstance.RecoverOnHit();

                    if (Player.MyInstance.IsOnBuff("Skill_Fire_02"))       // 발화 중일 시 디버프 생성
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
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;  // Math.Atan2 탄젠트 값으로 각도를 산출 https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
                                                                              // Mathf.Rad2Deg 라디안 각도 변환해주는 상수 http://jw910911.tistory.com/6
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);  // 축 중심 각도로 회전
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

    private bool CheckHitEnemy(Collider2D collision) // 스킬 한번 맞았으면 다시 안맞게 체크
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
            spell_1.Direction = new Vector2(Direction.x, Direction.y);  // 정방향
            SpellScript spell_2 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_2.Direction = new Vector2(Direction.y, -Direction.x);  // 우측
            SpellScript spell_3 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_3.Direction = new Vector2(-Direction.x, -Direction.y);  // 하단
            SpellScript spell_4 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_4.Direction = new Vector2(-Direction.y, Direction.x);  // 좌측

            SpellScript spell_5 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_5.Direction = new Vector2(Direction.x + Direction.y, Direction.y - Direction.x);  // 우측 상단
            SpellScript spell_6 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_6.Direction = new Vector2(Direction.y - Direction.x, -Direction.x - Direction.y);  // 우측 하단
            SpellScript spell_7 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_7.Direction = new Vector2(-Direction.x - Direction.y, -Direction.y + Direction.x);  // 좌측 하단
            SpellScript spell_8 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/P_Skill_Fire_08_Object"), transform.position, Quaternion.identity).GetComponent<SpellScript>();
            spell_8.Direction = new Vector2(-Direction.y + Direction.x, Direction.x + Direction.y);  // 좌측 상단
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
            //-- 활 회전 및 방향 설정 --
            // 오토 타겟팅
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

            // 화살 발사
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

        if (objects.Count == 0)     // 만약 리스트가 비었다면 null을 반환
            return null;

        var neareastObject = objects        // 거리가 가장 짧은 오브젝트를 구한다.
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