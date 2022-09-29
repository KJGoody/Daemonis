using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public StatBar HealthBar;       // ü�¹� �̹���
    public StatBar ManaBar;         // ������ �̹���
    public StatBar ExpBar;            // ����ġ �̹���

    [SerializeField] private int level;               // ����
    [SerializeField] private int levelUpEXP;          // ������ �ʿ� ����ġ
    [SerializeField] private float currentEXP;        // ����ġ
    [Header("�⺻ ����")]
    [SerializeField] private int attack;              // ���ݷ�
    [SerializeField] private float attackPercent;     // ���ݷ� %����
    [SerializeField] private int maxHealth;           // �ִ� ü��
    [SerializeField] private float maxHealthPercent;  // �ִ� ü�� %����
    [SerializeField] private int currentHealth;       // ���� ü��
    [SerializeField] private int maxMana;             // �ִ� ����
    [SerializeField] private float maxManaPercent;    // �ִ� ���� %����
    [SerializeField] private int currentMana;         // ���� ����

    [Header("�ΰ�����")]
    [SerializeField] private int defence;             // ���� ����
    [SerializeField] private float defencePercent;    // ���� ���� %����
    [SerializeField] private int magicRegist;         // ���� ����
    [SerializeField] private float magicRegistPercent;// ���� ���� %����
    [SerializeField] private float moveSpeed;         // ���� �̵��ӵ�
    [SerializeField] private float moveSpeedPercent;  // �̵��ӵ� %����
    [SerializeField] private float attackSpeed;       // ���ݼӵ�
    [SerializeField] private float attackSpeedPercent;// ���ݼӵ� %���� 
    [SerializeField] private float dodgePercent;      // ȸ�� Ȯ��
    [SerializeField] private float hitPercent;        // ���߷�
    [SerializeField] private float criticalPercent;   // ũ��Ƽ�� Ȯ��
    [SerializeField] private float criticalDamage;    // ũ��Ƽ�� ������
    [SerializeField] private int healthRegen;       // �ʴ� ü�� ���
    [SerializeField] private int manaRegen;         // �ʴ� ���� ���
    [SerializeField] private int recoverHealth_onhit;// ���߽� ü��ȸ��
    [SerializeField] private int recoverMana_onhit; // ���߽� ����ȸ��
    [SerializeField] private float coolDown;          // ����ð� ����
    [SerializeField] private float itemLootRange;     // ������ ȹ�� ����
    [SerializeField] private float lootRangePercent;  // ������ ȹ�� ���� %����
    [SerializeField] private float itemDropPercent;   // ������ ȹ�� Ȯ��
    [SerializeField] private float goldPlus;          // ��� ȹ�淮 ����
    [SerializeField] private float expPlus;           // ����ġ ȹ�淮 ����
    [SerializeField] private float vampiricRate;      // ������
    [SerializeField] private float potionCooldown;    // ���� ��Ÿ�Ӱ���

    public int Level // ����
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
    // ���ݷ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseAttack // ���ݷ� // ���̽� ��ġ
    {
        get { return attack; }
        set { attack = value; }
    }
    public int CurrentAttack // �ۼ�Ʈ ���� ����� ��ġ
    {
        get
        {
            if (attackPercent != 0)
                return (int)(attack + attack * (attackPercent / 100));
            else
                return attack;
        }
    }
    public float AttackPercent // �ۼ�Ʈ ���� ��ġ
    {
        get { return attackPercent; }
        set { attackPercent = value; }
    }

    // ü�� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseMaxHealth // �ִ� ü��
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
    public int CurrentHealth // ���� ü��
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

    // ���� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseMaxMana // �ִ� ����
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
    public int CurrentMana // ���� ����
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

    // ���� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseDefence // ���� ����
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

    // ���� ���� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int BaseMagicRegist // ���� ����
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

    // �̵��ӵ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float MoveSpeed // �̵��ӵ�
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

    // ���� �ӵ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float BaseAttackSpeed // ���� �ӵ�
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

    // ȸ�� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float DodgePercent // ȸ��
    {
        get { return dodgePercent; }
        set { dodgePercent = value; }
    }

    // ���� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float HitPercent // ����
    {
        get { return hitPercent; }
        set { hitPercent = value; }
    }

    // ũ��Ƽ�� Ȯ�� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float CriticalPercent // ũȮ
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

    // ũ��Ƽ�� ������ @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float CriticalDamage // ũ��
    {
        get { return criticalDamage; }
        set { criticalDamage = value; }
    }

    // ü�� ��� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int HealthRegen // ü��
    {
        get { return healthRegen; }
        set { healthRegen = value; }
    }

    // ���� ��� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public int ManaRegen // ����
    {
        get { return manaRegen; }
        set { manaRegen = value; }
    }

    public int RecoverHealth_onhit // ���߽� ü��
    {
        get { return recoverHealth_onhit; }
        set { recoverHealth_onhit = value; }
    }
    public int RecoverMana_onhit // ���߽� ����
    {
        get { return recoverMana_onhit; }
        set { recoverMana_onhit = value; }
    }
    public float CoolDown // ��
    {
        get { return coolDown; }
        set { coolDown = value; }
    }

    // ������ ȹ�� �ݰ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float BaseItemLootRange // ������ ȹ��ݰ�
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

    // ������ ȹ��� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public float ItemDropPercent // ��ȹ
    {
        get { return itemDropPercent; }
        set { itemDropPercent = value; }
    }
    public float GoldPlus // ��ȹ
    {
        get { return goldPlus; }
        set { goldPlus = value; }
    }
    public float ExpPlus // ����
    {
        get
        {
            if (expPlus < 0)
                return 0;
            return expPlus;
        }
        set { expPlus = value; }
    }
    public float VampiricRate // ����
    {
        get { return vampiricRate; }
        set { vampiricRate = value; }
    }
    public float PotionCoolDown // ������
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
