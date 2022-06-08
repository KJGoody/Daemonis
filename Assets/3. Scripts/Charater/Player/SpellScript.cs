using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    public enum SpellName
    {
        #region 스킬 이름
        Skill_File_01,
        Skill_File_03,
        Skill_File_04,
        Skill_File_05,
        Skill_File_06,
        Skill_File_07,
        Skill_File_08,
        Skill_File_09
        #endregion
    }
    [SerializeField]
    private SpellName spellName;
    private bool IsAOEAttack = false;
    private bool IsToggleAttack = false;

    [HideInInspector]
    public Transform MyTarget;    // 공격할 대상
    private Vector2 direction;
    [HideInInspector]
    public Vector2 atkDir;

    [SerializeField]
    private float speed;
    private float SpellxDamage;
    [HideInInspector]
    public float StackxDamage;

    public float CastTime;

    private Coroutine TickCoroutine;

    private List<GameObject> hitEnemy = new List<GameObject>();

    private void Start()
    {
        switch (spellName)
        {
            case SpellName.Skill_File_01:   // 화염구
                SoundManager.Instance.PlaySFXSound("Skill_Fire_01");
                SpellxDamage = 1;
                StartCoroutine(Skill_Fire_01());
                break;

            case SpellName.Skill_File_03:   // 용암 지대
                SoundManager.Instance.PlaySFXSoundLoop("Skill_Fire_04", transform);
                IsAOEAttack = true;
                SpellxDamage = 0.1f;
                StartCoroutine(Skill_Fire_03());
                break;

            case SpellName.Skill_File_04:   // 피닉스
                SoundManager.Instance.PlaySFXSound("Skill_Fire_04");
                SpellxDamage = 2;
                StartCoroutine(Skill_Fire_04());
                break;

            case SpellName.Skill_File_05:   // 화염 위성
                IsToggleAttack = true;
                SpellxDamage = 0.1f;
                StartCoroutine(Skill_Fire_05());
                break;

            case SpellName.Skill_File_07:   // 점화
                SoundManager.Instance.PlaySFXSound("Skill_Fire_07", 0.2f);
                SpellxDamage = 0.5f * StackxDamage;
                StartCoroutine(Skill_Fire_07());
                break;

            case SpellName.Skill_File_09:   // 화염 토네이도
                SoundManager.Instance.PlaySFXSoundLoop("Skill_Fire_09", transform);
                SpellxDamage = 0.2f;
                StartCoroutine(Skill_Fire_09());
                break;
        }

        if (IsToggleAttack || spellName.Equals(SpellName.Skill_File_07))        // 토글 공격은 리기드바디가 없음
            transform.position += new Vector3(0, 0.3f);                         // 토글 중심점 위치 변경
        else
            myRigidbody = GetComponent<Rigidbody2D>();

        if (MyTarget != null)
            direction = MyTarget.position - transform.position;
        else
        {
            direction = atkDir;
            if (direction == Vector2.zero)
            {
                if (Player.MyInstance._prefabs.transform.localScale.x == -1)    // 플레이어가 바라보고 있는 방향확인
                    direction = new Vector2(1, 0);                              // localscale이 -1일때 왼쪽을 보고 있음
                else
                    direction = new Vector2(-1, 0);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!spellName.Equals(SpellName.Skill_File_07))
        {
            if (MyTarget == null && spellName.Equals(SpellName.Skill_File_09))    // 토네이도 공격 시 오토타겟팅
            {
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    if (FindNearestObject() != null)
                        MyTarget = FindNearestObject().transform.Find("HitBox").transform;
                    else
                        MyTarget = null;
                }
            }

            if (spellName.Equals(SpellName.Skill_File_09))
            {
                if (MyTarget != null)
                    if (!MyTarget.parent.gameObject.GetComponent<EnemyBase>().IsAlive)
                        MyTarget = null;

                if (MyTarget != null)
                    direction = MyTarget.position - transform.position; // 타겟과 스펠의 방향과 크기 구함
                myRigidbody.velocity = direction.normalized * speed;    // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용
            }
            else if (!IsToggleAttack)
            {
                myRigidbody.velocity = direction.normalized * speed; // direction을 normalized하여 방향값으로 바꿔주고 발사하는 힘 적용
                                                                     // Math.Atan2 탄젠트 값으로 각도를 산출 https://m.blog.naver.com/PostView.nhn?blogId=sang9151&logNo=220821255191&categoryNo=50&proxyReferer=https%3A%2F%2Fwww.google.com%2F
                                                                     // Mathf.Rad2Deg 라디안 각도 변환해주는 상수 http://jw910911.tistory.com/6
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 축 중심 각도로 회전
            }
            else
                transform.Rotate(new Vector3(0, 0, Time.deltaTime * -300));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && !spellName.Equals(SpellName.Skill_File_04) && !spellName.Equals(SpellName.Skill_File_05) && !spellName.Equals(SpellName.Skill_File_09))     // 벽과 접촉시 파괴
            Destroy(gameObject);

        if (collision.CompareTag("Enemy") && !IsAOEAttack)
        {
            if (spellName.Equals(SpellName.Skill_File_09))
            {
                if (TickCoroutine == null)      // 토네이도에 접촉 시 코루틴 시작
                    TickCoroutine = StartCoroutine(TickDamage());
            }
            else
            {
                if (!CheckHitEnemy(collision))
                {
                    if (Player.MyInstance.IsOnBuff("Skill_Fire_02_Buff"))       // 발화 중일 시 디버프 생성
                        collision.transform.parent.GetComponent<EnemyBase>().NewBuff("Skill_Fire_02_Debuff");
                    SpendDamage(collision);
                    Player.MyInstance.RecoverOnHit();
                    if (!IsToggleAttack)
                        PuffPool.Instance.GetObject(PuffPool.PuffPrefabsName.Hit_01).PositioningPuff(transform.position);
                    else
                        PuffPool.Instance.GetObject(PuffPool.PuffPrefabsName.Hit_01).PositioningPuff(collision.transform.position);
                }
            }
        }
    }

    private void SpendDamage(Collider2D collision)
    {
        Character character = collision.GetComponentInParent<Character>();

        float WeaponxDamage;
        if (Player.MyInstance.usingEquipment[3] != null)
            WeaponxDamage = Player.MyInstance.usingEquipment[3].GetWeaponxDamage();
        else
            WeaponxDamage = 1;

        // 무기 배수    // 플레이어 공격력               // 스킬 배수
        float PureDamage = (WeaponxDamage * Player.MyInstance.MyStat.BaseAttack * SpellxDamage) * Player.MyInstance.BuffxDamage;
        character.TakeDamage(false, Player.MyInstance.MyStat.HitPercent, PureDamage, Player.MyInstance.MyStat.Level, direction, NewTextPool.NewTextPrefabsName.Enemy);
    }

    private bool CheckHitEnemy(Collider2D collision) // 스킬 한번 맞았으면 다시 안맞게 체크
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
        float AOERadius = 0.5f;             // 장판 범위
        int AOETimes = 5;                   // 장판 피격 횟수
        float AOEWaitForSeconds = 0.5f;     // 다음 피격 시간

        for (int i = 0; i < AOETimes; i++)
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, AOERadius, LayerMask.GetMask("HitBox"));
            if (collisions != null)
            {
                for (int j = 0; j < collisions.Length; j++)
                    if (collisions[j].CompareTag("Enemy"))
                    {
                        if (Player.MyInstance.IsOnBuff("Skill_Fire_02_Buff"))
                            collisions[j].transform.parent.GetComponent<EnemyBase>().NewBuff("Skill_Fire_02_Debuff");
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
            if (collisions[i].CompareTag("Enemy"))       // 테그가 적인것을 찾는다.
                objects.Add(collisions[i].gameObject);

        if (objects.Count == 0)     // 만약 리스트가 비었다면 null을 반환
            return null;

        var neareastObject = objects        // 거리가 가장 짧은 오브젝트를 구한다.
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
                        if (Player.MyInstance.IsOnBuff("Skill_Fire_02_Buff"))
                            collisions[j].transform.parent.GetComponent<EnemyBase>().NewBuff("Skill_Fire_02_Debuff");
                        SpendDamage(collisions[j]);
                        Player.MyInstance.RecoverOnHit();
                        PuffPool.Instance.GetObject(PuffPool.PuffPrefabsName.Hit_01).PositioningPuff(collisions[j].transform.position);
                    }
            }
            yield return new WaitForSeconds(WaitForSconds);
        }
    }
}