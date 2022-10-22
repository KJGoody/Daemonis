using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

[System.Serializable]
public class TargetGroup
{
    public string GroupName;
    public List<Transform> Targets = new List<Transform>();

    public TargetGroup(string groupName, Transform target)
    {
        GroupName = groupName;
        Targets.Add(target);
    }
}

public class Player : Character
{
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    // 장착중인 장비 아이템
    public Item_Equipment[] usingEquipment = new Item_Equipment[6];
    public delegate void UseEquipment(int partNum);
    public event UseEquipment useEquipment;

    private FloatingJoystick joy;  //조이스틱
    [SerializeField] private Transform exitPoint;  // 스킬 발사 위치
    [SerializeField] private GameObject lvUp_Particle;  // 레벨업 이펙트
    private Vector2 atkDir;  // 공격 방향

    private List<TargetGroup> targetGroups = new List<TargetGroup>();
    [SerializeField] private GameObject YOUDIEWindow;  // 캐릭터 사망 패널

    protected override void Start()
    {
        joy = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();

        NewBuff("Skill_Fire_02");

        MyStat.Level = GameManager.MyInstance.DATA.PlayerLevel;
        MyStat.SetLevelStat(MyStat.Level);
        MyStat.SetStat();

        base.Start();
    }
    protected override void Update()
    {
        GetInput();
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void GetInput() // 조이스틱 사용한 캐릭터 방향잡기
    {
        Vector2 moveVector;
        if (!IsAttacking)
        {
            moveVector.x = joy.Horizontal;
            moveVector.y = joy.Vertical;

            // 키보드 대응용 코드 //
            if (Input.GetKey(KeyCode.W))
                moveVector.y = 1;
            if (Input.GetKey(KeyCode.S))
                moveVector.y = -1;
            if (Input.GetKey(KeyCode.A))
                moveVector.x = -1;
            if (Input.GetKey(KeyCode.D))
                moveVector.x = 1;
            ////////////////////////
            
            Direction = moveVector;
            if (moveVector.x != 0 && moveVector.y != 0)
                atkDir = moveVector;

            if (IsMoving)
            {
                Animator runParticle = GetComponent<Animator>();
                runParticle.SetTrigger("Run");
            }
        }
    }

    public void RunParticle(GameObject runparticle)
    {
        Instantiate(runparticle, transform.position, Quaternion.identity);
    }

    public void CastSpell(string Spell_ID) // 스킬 사용
    {
        if (MyTarget != null)
            if (!MyTarget.parent.gameObject.GetComponent<EnemyBase>().IsAlive || MyTarget.parent.gameObject.activeSelf == false)
                MyTarget = null;

        if (MyTarget == null && SearchEnemy())
            AutoTarget();

        if (!IsAttacking)
            StartCoroutine(CastingSpell(Spell_ID));
    }

    private bool SearchEnemy() // 적이 있는지 검색
    {
        if (GameObject.FindWithTag("Enemy") == null)
            return false;
        else
            return true;
    }

    private void AutoTarget() // 자동 타겟팅
    {
        if (FindNearestObject() != null)
            MyTarget = FindNearestObject().transform;
        else
            MyTarget = null;
    }

    private GameObject FindNearestObject() // 가까운 적 타겟팅
    {
        // 확인 범위
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 7, LayerMask.GetMask("EnemyHitBox"));

        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < collisions.Length; i++)
            if (collisions[i].CompareTag("Enemy"))
                objects.Add(collisions[i].gameObject);

        if (objects.Count == 0) return null;

        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }

    private IEnumerator CastingSpell(string Spell_ID)
    {
        IsAttacking = true;

        // 타겟이 있다면 타겟 방향을 바라보도록한다.
        if (MyTarget != null) LookAtTarget();
        _prefabs.PlayAnimation(4);

        Spell newSpell = new Spell();
        newSpell.SetSpellInfo(DataTableManager.Instance.GetInfo_Spell(Spell_ID));
        if (newSpell.Type == SpellInfo.SpellType.Target)
        {
            switch (Spell_ID)
            {
                case "Skill_Fire_07":
                    for (int i = targetGroups.Count - 1; i >= 0; i--)
                        if (targetGroups[i].GroupName.Equals("Debuff_Skill_Fire_02"))
                            for (int j = targetGroups[i].Targets.Count - 1; j >= 0; j--)
                                if (Vector2.Distance(transform.position, targetGroups[i].Targets[j].position) < 7)
                                {
                                    SpellScript spellScript = Instantiate(newSpell.Prefab, targetGroups[i].Targets[j]).GetComponent<SpellScript>();
                                    spellScript.MyTarget = targetGroups[i].Targets[j];
                                    if (targetGroups[i].Targets[j].gameObject.activeSelf)
                                    {
                                        spellScript.StackxDamage = targetGroups[i].Targets[j].transform.GetComponent<EnemyBase>().GetBuff("Debuff_Skill_Fire_02").BuffStack;
                                        targetGroups[i].Targets[j].transform.GetComponent<EnemyBase>().OffBuff("Debuff_Skill_Fire_02");
                                    }
                                    else
                                        targetGroups[i].Targets.Remove(targetGroups[i].Targets[j]);
                                }
                    break;
            }

            yield return new WaitForSeconds(0.3f);
            IsAttacking = false;
        }
        else
        {
            SpellScript spellScript = InstantiateSpell(newSpell);
            spellScript.Direction = atkDir;
            if (MyTarget != null)
                spellScript.MyTarget = MyTarget;

            yield return new WaitForSeconds(0.3f);
            IsAttacking = false;
        }
    }

    private SpellScript InstantiateSpell(Spell spell)
    {
        switch (spell.Type)
        {
            case SpellInfo.SpellType.Launch:
                return Instantiate(spell.Prefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();

            case SpellInfo.SpellType.AOE:
                if (MyTarget != null)
                    return Instantiate(spell.Prefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
                else
                {
                    Vector3 ExitPoint = transform.position + new Vector3(atkDir.x, atkDir.y, 0).normalized * 2;
                    if (ExitPoint == transform.position)
                    {
                        if (_prefabs.transform.localScale.x == -1)
                            ExitPoint += new Vector3(1, 0, 0).normalized * 2;
                        else
                            ExitPoint += new Vector3(-1, 0, 0).normalized * 2;
                    }
                    return Instantiate(spell.Prefab, ExitPoint, Quaternion.identity).GetComponent<SpellScript>();
                }

            case SpellInfo.SpellType.Toggle:
                return Instantiate(spell.Prefab, transform).GetComponent<SpellScript>();

            case SpellInfo.SpellType.AE:
                if (MyTarget != null)
                    return Instantiate(spell.Prefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
                else
                {
                    Vector3 ExitPoint = transform.position + new Vector3(atkDir.x, atkDir.y, 0).normalized * 2;
                    if (ExitPoint == transform.position)
                    {
                        if (_prefabs.transform.localScale.x == -1)
                            ExitPoint += new Vector3(1, 0, 0).normalized * 2;
                        else
                            ExitPoint += new Vector3(-1, 0, 0).normalized * 2;
                    }
                    return Instantiate(spell.Prefab, ExitPoint, Quaternion.identity).GetComponent<SpellScript>();
                }

            case SpellInfo.SpellType.Turret:
                {
                    Vector3 ExitPoint = transform.position + new Vector3(atkDir.x, atkDir.y, 0).normalized * 2;
                    if (ExitPoint == transform.position)
                    {
                        if (_prefabs.transform.localScale.x == -1)
                            ExitPoint += new Vector3(1, 0, 0).normalized * 2;
                        else
                            ExitPoint += new Vector3(-1, 0, 0).normalized * 2;
                    }
                    return Instantiate(spell.Prefab, ExitPoint, Quaternion.identity).GetComponent<SpellScript>();
                }
        }
        return null;
    }

    public void AddTarget(string TargetGroupName, Transform Target)
    {
        bool IsAlreadyIn = false;
        int Index = 0;

        if (targetGroups.Count > 0)
        {
            for (int i = 0; i < targetGroups.Count; i++)
                if (targetGroups[i].GroupName.Equals(TargetGroupName))
                {
                    IsAlreadyIn = true;
                    Index = i;
                }

            if (IsAlreadyIn)
            {
                if (!targetGroups[Index].Targets.Contains(Target.transform))
                    targetGroups[Index].Targets.Add(Target.transform);
            }
            else
                targetGroups.Add(new TargetGroup(TargetGroupName, Target.transform));
        }
        else
            targetGroups.Add(new TargetGroup(TargetGroupName, Target.transform));
    }

    public void RemoveTarget(string TargetGroupName, Transform Target)
    {
        for (int i = targetGroups.Count - 1; i >= 0; i--)
            if (targetGroups[i].GroupName.Equals(TargetGroupName))
                if (targetGroups[i].Targets.Contains(Target))
                {
                    targetGroups[i].Targets.Remove(Target);
                    if (targetGroups[i].Targets.Count == 0)
                        targetGroups.Remove(targetGroups[i]);
                }
    }

    public void EquipItem(Item_Equipment newItem) // 장비 장착
    {
        int partNum = 0;
        switch (newItem.Part)
        {
            case ItemInfo_Equipment.Parts.Helmet:
                partNum = 0;
                break;
            case ItemInfo_Equipment.Parts.Cloth:
                partNum = 1;
                break;
            case ItemInfo_Equipment.Parts.Shoes:
                partNum = 2;
                break;
            case ItemInfo_Equipment.Parts.Weapon:
                partNum = 3;
                break;
            case ItemInfo_Equipment.Parts.Shoulder:
                partNum = 4;
                break;
            case ItemInfo_Equipment.Parts.Back:
                partNum = 5;
                break;
        }
        if (usingEquipment[partNum] != null)
        {
            InventoryScript.MyInstance.AddItem(usingEquipment[partNum]);
        }
        usingEquipment[partNum] = newItem;
        _spriteList.ChangeItem(partNum);
        useEquipment(partNum);
        newItem.ActiveEquipment(true);
    }

    public void UnequipItem(int partNum) // 장비 해제
    {
        usingEquipment[partNum].ActiveEquipment(false);
        InventoryScript.MyInstance.AddItem(usingEquipment[partNum]);
        usingEquipment[partNum] = null;
        _spriteList.ChangeItem(partNum);
    }

    public void PlusStat(string option, float value) // 추가옵션 스탯 적용
    {
        // stat에서 option과 같은 이름의 변수를 가지고 온다.
        PropertyInfo optionName = stat.GetType().GetProperty(option);

        float b = (float)System.Convert.ToDouble(optionName.GetValue(stat));
        switch (option)
        {
            case "BaseAttack":
            case "BaseMaxHealth":
            case "BaseMaxMana":
            case "BaseDefence":
            case "BaseMagicRegist":
            case "BaseAttackSpeed":
            case "HealthRegen":
            case "ManaRegen":
            case "RecoverHealth_onhit":
            case "RecoverMana_onhit":
                optionName.SetValue(stat, (int)(b + value));
                break;

            default:
                optionName.SetValue(stat, (float)(b + value));
                break;
        }
    }

    public void SpendEXP(float MonsterExP, bool Repeat = false) // 경험치 증가
    {
        float EXP;
        if (Repeat)
            EXP = MonsterExP;
        else
            EXP = MonsterExP + MonsterExP * (MyStat.ExpPlus / 100f);

        if (MyStat.LevelUpEXP > MyStat.CurrentEXP + EXP)
            MyStat.CurrentEXP += EXP;
        else
        {
            float surPlusEXP = MyStat.CurrentEXP + EXP - MyStat.LevelUpEXP;
            MyStat.Level++;
            MyStat.SetLevelStat(MyStat.Level);
            MyStat.CurrentHealth = MyStat.CurrentMaxHealth;
            MyStat.CurrentMana = MyStat.CurrentMaxMana;
            MyStat.CurrentEXP = 0;
            MyStat.ExpBar.Initialize(MyStat.LevelUpEXP, MyStat.CurrentEXP);
            Instantiate(lvUp_Particle, transform).transform.SetParent(transform);
            SpendEXP(surPlusEXP, true);
        }
    }

    public override void TakeDamage(DamageType damageType, float HitPercent, float pureDamage, int FromLevel, Vector2 knockbackDir, NewTextPool.NewTextPrefabsName TextType, AttackType attackType = AttackType.Normal)
    {
        base.TakeDamage(damageType, HitPercent, pureDamage, FromLevel, knockbackDir, TextType);

        if (MyStat.CurrentHealth <= 0)
        {
            IsAlive = false;
            YOUDIEWindow.SetActive(true);
            transform.Find("HitBox_Player").gameObject.SetActive(false);
            myRigid2D.simulated = false;
            // 참조) 되살아는 부븐 PortalManager._LoadSceneName
        }
    }
}
