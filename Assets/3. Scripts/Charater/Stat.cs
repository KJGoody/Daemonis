using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public StatBar HealthBar;       // 체력바 이미지
    public StatBar ManaBar;         // 마나바 이미지
    public StatBar ExpBar;            // 경험치 이미지

    [SerializeField] private int level;               // 레벨
    [SerializeField] private int levelUpEXP;          // 레벨업 필요 경험치
    [SerializeField] private float currentEXP;        // 경험치
    [Header("기본 스탯")]
    [SerializeField] private int attack;              // 공격력
    [SerializeField] private float attackPercent;     // 공격력 %증가
    [SerializeField] private int maxHealth;           // 최대 체력
    [SerializeField] private float maxHealthPercent;  // 최대 체력 %증가
    [SerializeField] private int currentHealth;       // 현재 체력
    [SerializeField] private int maxMana;             // 최대 마나
    [SerializeField] private float maxManaPercent;    // 최대 마나 %증가
    [SerializeField] private int currentMana;         // 현재 마나

    [Header("부가스탯")]
    [SerializeField] private int defence;             // 물리 방어력
    [SerializeField] private float defencePercent;    // 물리 방어력 %증가
    [SerializeField] private int magicRegist;         // 마법 방어력
    [SerializeField] private float magicRegistPercent;// 마법 방어력 %증가
    [SerializeField] private float moveSpeed;         // 실제 이동속도
    [SerializeField] private float moveSpeedPercent;  // 이동속도 %증가
    [SerializeField] private float attackSpeed;       // 공격속도
    [SerializeField] private float attackSpeedPercent;// 공격속도 %증가 
    [SerializeField] private float dodgePercent;      // 회피 확률
    [SerializeField] private float hitPercent;        // 명중률
    [SerializeField] private float criticalPercent;   // 크리티컬 확률
    [SerializeField] private float criticalDamage;    // 크리티컬 데미지
    [SerializeField] private int healthRegen;       // 초당 체력 재생
    [SerializeField] private int manaRegen;         // 초당 마나 재생
    [SerializeField] private int recoverHealth_onhit;// 적중시 체력회복
    [SerializeField] private int recoverMana_onhit; // 적중시 마나회복
    [SerializeField] private float coolDown;          // 재사용시간 감소
    [SerializeField] private float itemLootRange;     // 아이템 획득 범위
    [SerializeField] private float lootRangePercent;  // 아이템 획득 범위 %증가
    [SerializeField] private float itemDropPercent;   // 아이템 획득 확률
    [SerializeField] private float goldPlus;          // 골드 획득량 증가
    [SerializeField] private float expPlus;           // 경험치 획득량 증가
    [SerializeField] private float vampiricRate;      // 흡혈률
    [SerializeField] private float potionCooldown;    // 포션 쿨타임감소

    public int Level // 레벨
    {
        get { return level; }
        set { level = value; }
    }
    public int LevelUpEXP
    {
        get { return levelUpEXP; }
        set { levelUpEXP = value; }
    }
    public float CurrentEXP
    {
        get { return currentEXP; }
        set 
        {
            currentEXP = value;
            ExpBar.StatBarCurrentValue = value;
        }
    }
    // 공격력 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseAttack // 공격력 // 베이스 수치
    {
        get { return attack; }
        set { attack = value; }
    }
    public int CurrentAttack // 퍼센트 증가 적용된 수치
    {
        get
        {
            if (attackPercent != 0)
                return (int)(attack + attack * (attackPercent / 100));
            else
                return attack;
        }
    }
    public float AttackPercent // 퍼센트 증가 수치
    {
        get { return attackPercent; }
        set { attackPercent = value; }
    }

    // 체력 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseMaxHealth // 최대 체력
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public int CurrentMaxHealth
    {
        get
        {
            if (maxHealthPercent != 0)
            {
                return (int)(maxHealth + maxHealth * (maxHealthPercent / 100));
            }
            else
            {
                return maxHealth;
            }
        }
    }
    public float MaxHealthPercent
    {
        get { return maxHealthPercent; }
        set { maxHealthPercent = value; }
    }
    public int CurrentHealth // 현재 체력
    {
        get { return currentHealth; }
        set
        {
            if (value > CurrentMaxHealth)
            {
                currentHealth = CurrentMaxHealth;
                HealthBar.StatBarCurrentValue = CurrentMaxHealth;
            }
            else if (value < 0)
            {
                currentHealth = 0;
                HealthBar.StatBarCurrentValue = 0;
            }
            else
            {
                currentHealth = value;
                HealthBar.StatBarCurrentValue = value;
            }
        }
    }

    // 마나 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseMaxMana // 최대 마나
    {
        get { return maxMana; }
        set { maxMana = value; }
    }
    public int CurrentMaxMana
    {
        get
        {
            if (maxManaPercent != 0)
                return (int)(maxMana + maxMana * (maxManaPercent / 100));
            else
                return (int)maxMana;
        }
    }
    public float MaxManaPercent
    {
        get { return maxManaPercent; }
        set { maxManaPercent = value; }
    }
    public int CurrentMana // 현재 마나
    {
        get { return currentMana; }
        set
        {
            if (value > CurrentMaxMana)
            {
                currentMana = CurrentMaxMana;
                ManaBar.StatBarCurrentValue = CurrentMaxMana;
            }
            else if (value < 0)
            {
                currentMana = 0;
                ManaBar.StatBarCurrentValue = 0;
            }
            else
            {
                currentMana = value; 
                ManaBar.StatBarCurrentValue = value;
            }
        }
    }

    // 방어력 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseDefence // 물리 방어력
    {
        get { return defence; }
        set { defence = value; }
    }
    public float CurrentDefence
    {
        get
        {
            if (defencePercent != 0)
                return (int)(defence + defence * (defencePercent / 100));
            else
                return (int)defence;
        }
    }
    public float DefencePercent
    {
        get { return defencePercent; }
        set { defencePercent = value; }
    }

    // 마법 방어력 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseMagicRegist // 마법 방어력
    {
        get { return magicRegist; }
        set { magicRegist = value; }
    }
    public float CurrentMagicRegist
    {
        get
        {
            if (magicRegistPercent != 0)
                return (int)(magicRegist + magicRegist * (magicRegistPercent / 100));
            else
                return (int)magicRegist;
        }
    }
    public float MagicRegistPercent
    {
        get { return magicRegistPercent; }
        set { magicRegistPercent = value; }
    }

    // 이동속도 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float MoveSpeed // 이동속도
    {
        get
        {
            if (moveSpeedPercent != 0)
                return moveSpeed + moveSpeed * (moveSpeedPercent / 100);
            else
                return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }
    public float MoveSpeedPercent
    {
        get { return moveSpeedPercent; }
        set
        {
            if (value > 200)
                moveSpeedPercent = 200;
            else
                moveSpeedPercent = value;
        }
    }

    // 공격 속도 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float BaseAttackSpeed // 공격 속도
    {
        get { return attackSpeed; }
        set
        {
            if (value < 0.1)
                attackSpeed = 0.1f;
            else
                attackSpeed = value;
        }
    }
    public float CurrentAttackSpeed
    {
        get
        {
            if (attackSpeedPercent != 0)
                return attackSpeed - attackSpeed * (attackSpeedPercent / 100);
            else
                return attackSpeedPercent;
        }
    }
    public float AttackSpeedPercent
    {
        get { return attackSpeedPercent; }
        set
        {
            if (value > 90)
                attackSpeedPercent = 90;
            else
                attackSpeedPercent = value;
        }
    }

    // 회피 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float DodgePercent // 회피
    {
        get { return dodgePercent; }
        set { dodgePercent = value; }
    }

    // 적중 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float HitPercent // 적중
    {
        get { return hitPercent; }
        set { hitPercent = value; }
    }

    // 크리티컬 확률 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float CriticalPercent // 크확
    {
        get { return criticalPercent; }
        set
        {
            if (value > 100)
            {
                criticalPercent = 100f;
            }
            else
            {
                criticalPercent = value;
            }
        }
    }

    // 크리티컬 데미지 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float CriticalDamage // 크댐
    {
        get { return criticalDamage; }
        set { criticalDamage = value; }
    }

    // 체력 재생 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int HealthRegen // 체젠
    {
        get { return healthRegen; }
        set { healthRegen = value; }
    }

    // 마나 재생 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int ManaRegen // 마젠
    {
        get { return manaRegen; }
        set { manaRegen = value; }
    }

    public int RecoverHealth_onhit // 적중시 체력
    {
        get { return recoverHealth_onhit; }
        set { recoverHealth_onhit = value; }
    }
    public int RecoverMana_onhit // 적중시 마나
    {
        get { return recoverMana_onhit; }
        set { recoverMana_onhit = value; }
    }
    public float CoolDown // 쿨감
    {
        get { return coolDown; }
        set { coolDown = value; }
    }

    // 아이템 획득 반경 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float BaseItemLootRange // 아이템 획득반경
    {
        get { return itemLootRange; }
        set { itemLootRange = value; }
    }
    public float CurrentItemLootRange
    {
        get
        {
            if (lootRangePercent != 0)
                return (itemLootRange + itemLootRange * (lootRangePercent / 100));
            else
                return itemLootRange;
        }
    }
    public float ItemLootRangePercent
    {
        get { return lootRangePercent; }
        set { lootRangePercent = value; }
    }

    // 아이템 획득률 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float ItemDropPercent // 아획
    {
        get { return itemDropPercent; }
        set { itemDropPercent = value; }
    }
    public float GoldPlus // 골획
    {
        get { return goldPlus; }
        set { goldPlus = value; }
    }
    public float ExpPlus // 경추
    {
        get
        {
            if (expPlus < 0)
                return 0;
            return expPlus;
        }
        set { expPlus = value; }
    }
    public float VampiricRate // 흡혈
    {
        get { return vampiricRate; }
        set { vampiricRate = value; }
    }
    public float PotionCoolDown // 포션쿨감
    {
        get { return potionCooldown; }
        set
        {
            if (value > 100)
                potionCooldown = 100f;
            else
                potionCooldown = value;
        }
    }

    public void SetStat()
    {
        currentHealth = CurrentMaxHealth;
        HealthBar.Initialize(CurrentMaxHealth, CurrentMaxHealth);

        if (ManaBar != null)
        {
            currentMana = CurrentMaxMana;
            ManaBar.Initialize(CurrentMaxMana, CurrentMaxMana);
        }

        if(ExpBar != null)
        {
            currentEXP = 0;
            ExpBar.Initialize(LevelUpEXP, currentEXP, true);
        }
    }

    public void SetHpMP()
    {
        HealthBar.SetMax(CurrentMaxHealth);
        ManaBar.SetMax(CurrentMaxMana);
    }

    public void InitializeHealth()
    {
        currentHealth = CurrentMaxHealth;
        HealthBar.Initialize(CurrentMaxHealth, CurrentMaxHealth);
    }
    private void Update()
    {
        levelUpEXP = level * 10;
        if(ExpBar != null)
            ExpBar.SetMax(levelUpEXP);
    }
}
