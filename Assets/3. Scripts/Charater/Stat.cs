using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [Header("기본 스탯")]
    public StatBar HealthBar;                           // 체력바 이미지
    public float MaxHealth;                             // 최대 체력
    private float currentHealth;                        // 현재 체력
    public float CurrentHealth
    {
        get { return currentHealth; }
        set 
        { 
            currentHealth = value; 
            HealthBar.StatBarCurrentValue = value; 
        }
    }
    public float HealthRegen;                           // 체력 재생

    public StatBar ManaBar;                             // 마나바 이미지
    public float MaxMana;                               // 최대 마나
    private float currentMana;                          // 현재 마나
    public float CurrentMana
    {
        get { return currentMana; }
        set 
        { 
            currentMana = value;
            ManaBar.StatBarCurrentValue = value; 
        }
    }
    public float ManaRegen;                             // 마나 재생

    public int Attack;              // 공격력
    public int Defence;             // 물리 방어력
    public int MagicRegist;         // 마법 방어력
    public float Speed;             // 이동속도
    public float AttackSpeed;       // 공격속도

    [Header("부가 스탯")]
    public float DodgePercent;      // 회피 확률
    public float HitPercent;        // 명중률
    public float CriticalPercent;   // 크리티컬 확률
    public float CriticalDamage;    // 크리티컬 데미지
    public float CoolDown;          // 재사용시간 감소



    private void Awake()
    {
        currentHealth = MaxHealth;
        HealthBar.Initialize(MaxHealth, MaxHealth);
        if (ManaBar != null)
            ManaBar.Initialize(MaxMana, MaxMana);
    }
}
