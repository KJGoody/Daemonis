using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [Header("±‚∫ª Ω∫≈»")]
    public StatBar HealthBar;
    public float MaxHealth;
    private float currentHealth;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            HealthBar.StatBarCurrentValue = value;
        }
    }
    public float HealthRegen;

    public StatBar ManaBar;
    public float MaxMana;
    private float currentMana;
    public float CurrentMana
    {
        get
        {
            return currentMana;
        }
        set
        {
            currentMana = value;
            ManaBar.StatBarCurrentValue = value;
        }
    }
    public float ManaRegen;

    public int Attack;
    public int Defence;
    public int MagicRegist;
    public float Speed;
    public float AttackSpeed;

    [Header("∫Œ∞° Ω∫≈»")]
    public float DodgePercent;
    public float HitPercent;
    public float CriticalPercent;
    public float CriticalDamage;
    public float CoolDown;



    private void Awake()
    {
        currentHealth = MaxHealth;
        HealthBar.Initialize(MaxHealth, MaxHealth);
        if (ManaBar != null)
            ManaBar.Initialize(MaxMana, MaxMana);
    }
}
