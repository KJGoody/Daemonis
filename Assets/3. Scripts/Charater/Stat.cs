using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public StatBar HealthBar;       // ü�¹� �̹���
    public StatBar ManaBar;         // ������ �̹���
    
    [Header("�⺻ ����")]
    [SerializeField] private int attack;              // ���ݷ�
    [SerializeField] private float maxHealth;         // �ִ� ü��
    [SerializeField] private float maxMana;           // �ִ� ����
    [SerializeField] private float currentHealth;    // ���� ü��
    [SerializeField] private float currentMana;      // ���� ����

    [Header("�ΰ�����")]
    [SerializeField] private int defence;             // ���� ����
    [SerializeField] private int magicRegist;         // ���� ����
    [SerializeField] private float moveSpeed;             // �̵��ӵ�
    [SerializeField] private float attackSpeed;       // ���ݼӵ�
    [SerializeField] private float dodgePercent;      // ȸ�� Ȯ��
    [SerializeField] private float hitPercent;        // ���߷�
    [SerializeField] private float criticalPercent;   // ũ��Ƽ�� Ȯ��
    [SerializeField] private float criticalDamage;    // ũ��Ƽ�� ������
    [SerializeField] private float healthRegen;       // �ʴ� ü�� ���
    [SerializeField] private float manaRegen;         // �ʴ� ���� ���
    [SerializeField] private float recoverHealth_onhit;// ���߽� ü��ȸ��
    [SerializeField] private float recoverMana_onhit; // ���߽� ����ȸ��
    [SerializeField] private float coolDown;          // ����ð� ����
    [SerializeField] private float itemLootRange;     // ������ ȹ�� ����
    [SerializeField] private float itemDropPercent;   // ������ ȹ�� Ȯ��
    [SerializeField] private float goldPlus;          // ��� ȹ�淮 ����
    [SerializeField] private float expPlus;           // ����ġ ȹ�淮 ����
    [SerializeField] private float vampiricRate;      // ������

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
