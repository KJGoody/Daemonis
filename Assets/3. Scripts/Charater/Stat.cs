using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [Header("�⺻ ����")]
    public StatBar HealthBar;                           // ü�¹� �̹���
    public float MaxHealth;                             // �ִ� ü��
    private float currentHealth;                        // ���� ü��
    public float CurrentHealth
    {
        get { return currentHealth; }
        set 
        { 
            currentHealth = value; 
            HealthBar.StatBarCurrentValue = value; 
        }
    }
    public float HealthRegen;                           // ü�� ���

    public StatBar ManaBar;                             // ������ �̹���
    public float MaxMana;                               // �ִ� ����
    private float currentMana;                          // ���� ����
    public float CurrentMana
    {
        get { return currentMana; }
        set 
        { 
            currentMana = value;
            ManaBar.StatBarCurrentValue = value; 
        }
    }
    public float ManaRegen;                             // ���� ���

    public int Attack;              // ���ݷ�
    public int Defence;             // ���� ����
    public int MagicRegist;         // ���� ����
    public float Speed;             // �̵��ӵ�
    public float AttackSpeed;       // ���ݼӵ�

    [Header("�ΰ� ����")]
    public float DodgePercent;      // ȸ�� Ȯ��
    public float HitPercent;        // ���߷�
    public float CriticalPercent;   // ũ��Ƽ�� Ȯ��
    public float CriticalDamage;    // ũ��Ƽ�� ������
    public float CoolDown;          // ����ð� ����



    private void Awake()
    {
        currentHealth = MaxHealth;
        HealthBar.Initialize(MaxHealth, MaxHealth);
        if (ManaBar != null)
            ManaBar.Initialize(MaxMana, MaxMana);
    }
}
