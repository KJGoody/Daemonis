using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public StatBar HealthBar;       // 체력바 이미지
    public StatBar ManaBar;         // 마나바 이미지
    
    [Header("기본 스탯")]
    [SerializeField] private int attack;              // 공격력
    [SerializeField] private float maxHealth;         // 최대 체력
    [SerializeField] private float maxMana;           // 최대 마나
    [SerializeField] private float currentHealth;    // 현재 체력
    [SerializeField] private float currentMana;      // 현재 마나

    [Header("부가스탯")]
    [SerializeField] private int defence;             // 물리 방어력
    [SerializeField] private int magicRegist;         // 마법 방어력
    [SerializeField] private float moveSpeed;             // 이동속도
    [SerializeField] private float attackSpeed;       // 공격속도
    [SerializeField] private float dodgePercent;      // 회피 확률
    [SerializeField] private float hitPercent;        // 명중률
    [SerializeField] private float criticalPercent;   // 크리티컬 확률
    [SerializeField] private float criticalDamage;    // 크리티컬 데미지
    [SerializeField] private float healthRegen;       // 초당 체력 재생
    [SerializeField] private float manaRegen;         // 초당 마나 재생
    [SerializeField] private float recoverHealth_onhit;// 적중시 체력회복
    [SerializeField] private float recoverMana_onhit; // 적중시 마나회복
    [SerializeField] private float coolDown;          // 재사용시간 감소
    [SerializeField] private float itemLootRange;     // 아이템 획득 범위
    [SerializeField] private float itemDropPercent;   // 아이템 획득 확률
    [SerializeField] private float goldPlus;          // 골드 획득량 증가
    [SerializeField] private float expPlus;           // 경험치 획득량 증가
    [SerializeField] private float vampiricRate;      // 흡혈률

    public float CurrentHealth
    {
        get { return currentHealth; }
        set 
        { 
            currentHealth = value; 
            HealthBar.StatBarCurrentValue = value; 
        }
    }
    public float CurrentMana
    {
        get { return currentMana; }
        set 
        { 
            currentMana = value;
            ManaBar.StatBarCurrentValue = value; 
        }
    }
    public int Attak
    {
        get { return attack; }
        set { attack = value; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public float MaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }
    public int Defence
    {
        get { return defence; }
        set { defence = value; }
    }
    public int MagicRegist
    {
        get { return magicRegist; }
        set { magicRegist = value; }
    }
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set 
        {
            if(value > 200)
            {
                moveSpeed = 200;
            }
            else
            {
            moveSpeed = value;
            }
        } 
    }
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            if (value < 0.1)
            {
                attackSpeed = 0.1f;
            }
            else
            {
                attackSpeed = value;
            }
        }
    }
    public float DodgePercent
    {
        get { return dodgePercent; }
        set { dodgePercent = value; }
    }
    public float HitPercent
    {
        get { return hitPercent; }
        set { hitPercent = value; }
    }
    public float CriticalPercent
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
    public float CriticalDamage
    {
        get { return criticalDamage; }
        set { criticalDamage = value; }
    }
    public float HealthRegen
    {
        get { return healthRegen; }
        set { healthRegen = value; }
    }
    public float ManaRegen
    {
        get { return manaRegen; }
        set { manaRegen = value; }
    }
    public float RecoverHealth_onhit
    {
        get { return recoverHealth_onhit; }
        set { recoverHealth_onhit = value; }
    }
    public float RecoverMana_onhit
    {
        get { return recoverMana_onhit; }
        set { recoverMana_onhit = value; }
    }
    public float CoolDown
    {
        get { return coolDown; }
        set { coolDown = value; }
    }
    public float ItemLootRange
    {
        get { return itemLootRange; }
        set { itemLootRange = value; }
    }
    public float ItemDropPercent
    {
        get { return itemDropPercent; }
        set { itemDropPercent = value; }
    }
    public float GoldPlus
    {
        get { return goldPlus; }
        set { goldPlus = value; }
    }
    public float ExpPlus
    {
        get { return expPlus; }
        set { expPlus = value; }
    }
    public float VampiricRate
    {
        get { return vampiricRate; }
        set { vampiricRate = value; }
    }

    private void Awake()
    {
        currentHealth = MaxHealth;
        HealthBar.Initialize(MaxHealth, MaxHealth);
        if (ManaBar != null)
            ManaBar.Initialize(MaxMana, MaxMana);
    }
}
