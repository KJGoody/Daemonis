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

    public ItemBase[] usingEquipment = new ItemBase[6];
    public delegate void UseEquipment(int partNum);
    public event UseEquipment useEquipment;

    private FloatingJoystick joy;
    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private GameObject lvUp_Particle;
    [HideInInspector]
    public Vector2 atkDir;

    private List<TargetGroup> targetGroups = new List<TargetGroup>();
    [SerializeField]
    private GameObject YOUDIEWindow;

    protected override void Start()
    {
        joy = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();

        NewBuff("Skill_Fire_02_Buff");

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

    private void GetInput()
    {
        Vector2 moveVector;
        if (!IsAttacking)
        {
            moveVector.x = joy.Horizontal;
            moveVector.y = joy.Vertical;

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

    public void CastSpell(string spellIName)
    {
        if (MyTarget != null)
            if (!MyTarget.parent.gameObject.GetComponent<EnemyBase>().IsAlive)
                MyTarget = null;

        if (MyTarget == null && SearchEnemy()) 
            AutoTarget();
        
        if (!IsAttacking)
            StartCoroutine(CastingSpell(spellIName));
    }

    private bool SearchEnemy()
    {
        if (GameObject.FindWithTag("Enemy") == null)
            return false;
        else
            return true;
    }

    private void AutoTarget()
    {
        if (FindNearestObject() != null)
            MyTarget = FindNearestObject().transform;
        else
            MyTarget = null;
    }

    private GameObject FindNearestObject()
    {
                                                                                 // 확인 범위
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 7, LayerMask.GetMask("HitBox"));

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

    private IEnumerator CastingSpell(string spellIName)
    {
        IsAttacking = true;
        if (MyTarget != null) LookAtTarget();
        _prefabs.PlayAnimation(4);

        Spell newSpell = SpellBook.MyInstance.GetSpell(spellIName);
        if (newSpell.spellType.Equals(Spell.SpellType.Immediate))
        {   // 점화 스킬 구현방식
            for (int i = targetGroups.Count - 1; i >= 0; i--)
            {
                if (targetGroups[i].GroupName.Equals("Skill_Fire_02_Debuff"))
                    for (int j = targetGroups[i].Targets.Count - 1; j >= 0; j--)
                    {
                        if (Vector2.Distance(transform.position, targetGroups[i].Targets[j].position) < 7)
                        {
                            SpellScript spellScript = Instantiate(newSpell.MySpellPrefab, targetGroups[i].Targets[j]).GetComponent<SpellScript>();
                            spellScript.MyTarget = targetGroups[i].Targets[j];
                            spellScript.StackxDamage = targetGroups[i].Targets[j].transform.GetComponent<EnemyBase>().GetBuff("Skill_Fire_02_Debuff").BuffStack;
                            targetGroups[i].Targets[j].transform.GetComponent<EnemyBase>().OffBuff("Skill_Fire_02_Debuff");
                        }
                    }
            }

            yield return new WaitForSeconds(0.3f);
            IsAttacking = false;
        }
        else
        {
            SpellScript spellScript = InstantiateSpell(newSpell);
            spellScript.atkDir = atkDir;
            if (MyTarget != null)
                spellScript.MyTarget = MyTarget;

            yield return new WaitForSeconds(spellScript.CastTime);
            IsAttacking = false;
        }
    }

    private SpellScript InstantiateSpell(Spell spell)
    {
        switch (spell.spellType)
        {
            case Spell.SpellType.Launch:
                return Instantiate(spell.MySpellPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();

            case Spell.SpellType.AOE:
                if (MyTarget != null)
                    return Instantiate(spell.MySpellPrefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
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
                    return Instantiate(spell.MySpellPrefab, ExitPoint, Quaternion.identity).GetComponent<SpellScript>();
                }

            case Spell.SpellType.Toggle:
                return Instantiate(spell.MySpellPrefab, transform).GetComponent<SpellScript>();

            case Spell.SpellType.AE:
                if (MyTarget != null)
                    return Instantiate(spell.MySpellPrefab, MyTarget.position, Quaternion.identity).GetComponent<SpellScript>();
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
                    return Instantiate(spell.MySpellPrefab, ExitPoint, Quaternion.identity).GetComponent<SpellScript>();
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

    public void EquipItem(ItemBase newItem)
    {
        int partNum = 0;
        switch (newItem.GetPart)
        {
            case Part.Helmet:
                partNum = 0;
                break;
            case Part.Cloth:
                partNum = 1;
                break;
            case Part.Shoes:
                partNum = 2;
                break;
            case Part.Weapon:
                partNum = 3;
                break;
            case Part.Shoulder:
                partNum = 4;
                break;
            case Part.Back:
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

    public void UnequipItem(int partNum)
    {
        usingEquipment[partNum].ActiveEquipment(false);
        InventoryScript.MyInstance.AddItem(usingEquipment[partNum]);
        usingEquipment[partNum] = null;
        _spriteList.ChangeItem(partNum);
    }

    public void Plus(string option, float value)
    {
        PropertyInfo optionName = stat.GetType().GetProperty(option);
        //float a = (float)System.Convert.ToDouble(optionName.GetValue(stat));
        float b = (float)System.Convert.ToDouble(optionName.GetValue(stat));
        switch (option)
        {
            case "BaseAttack": case "BaseMaxHealth": case "BaseMaxMana": case "BaseDefence":
            case "BaseMagicRegist": case "BaseAttackSpeed" : case "HealthRegen":
            case "ManaRegen": case "RecoverHealth_onhit": case "RecoverMana_onhit":
                optionName.SetValue(stat, (int)(b + value));
                break;
            default:
                optionName.SetValue(stat, (float)(b + value));
                break;

        }
    }

    public void SpendEXP(float MonsterExP, bool Repeat = false)
    {
        float EXP;
        if (Repeat)
            EXP = MonsterExP;
        else
            EXP = MonsterExP * MyStat.ExpPlus;

        if (MyStat.LevelUpEXP > MyStat.CurrentEXP + EXP)
            MyStat.CurrentEXP += EXP;
        else
        {
            float surPlusEXP = MyStat.CurrentEXP + EXP - MyStat.LevelUpEXP;
            MyStat.Level++;
            MyStat.CurrentEXP = 0;
            MyStat.ExpBar.Initialize(MyStat.LevelUpEXP, MyStat.CurrentEXP);
            Instantiate(lvUp_Particle, transform).transform.SetParent(transform);
            SpendEXP(surPlusEXP, true);
        }
    }

    public override void TakeDamage(bool IsPhysic, float HitPercent, float pureDamage, int FromLevel, Vector2 knockbackDir, NewTextPool.NewTextPrefabsName TextType)
    {
        base.TakeDamage(IsPhysic, HitPercent, pureDamage, FromLevel, knockbackDir, TextType);

        if(MyStat.CurrentHealth <= 0)
        {
            YOUDIEWindow.SetActive(true);
            transform.Find("HitBox_Player").gameObject.SetActive(false);
            myRigid2D.simulated = false;
        }
    }
}
