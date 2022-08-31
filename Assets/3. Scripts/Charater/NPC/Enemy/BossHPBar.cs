using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    private static BossHPBar instance;
    public static BossHPBar Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BossHPBar>();
            return instance;
        }
    }

    [SerializeField] private Text BossName;
    [SerializeField] private GameObject CrownIcon;
    [SerializeField] private Image BossHPBarImage;
    [SerializeField] private Text BossHPBarText;

    private EnemyBase Parent;

    private float CurrentFill;
    private float BossHPMaxValue;
    private float CurrenBossHPValue;

    private bool IsFix;

    private void Update()
    {
        if (CurrentFill != BossHPBarImage.fillAmount)
            BossHPBarImage.fillAmount = Mathf.Lerp(BossHPBarImage.fillAmount, CurrentFill, Time.deltaTime);
    }

    public void BossHPBarSetActive(bool setactive)
    {
        if (setactive) this.GetComponent<CanvasGroup>().alpha = 1;
        else this.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void BossHPBarSetActive(bool setactive, EnemyBase parent, bool Fix = false, Character.AttackType attackType = Character.AttackType.Normal)
    {
        // 체력바 표시 대상 고정
        if (Fix)
            if (setactive)
            {
                Parent = parent;
                SetValue();
                InitializeBossHPBar();
                IsFix = true;
            }
            else
            {
                Parent = null;
                IsFix = false;
            }

        if (setactive)
        {
            SetBossHP(parent, attackType);
            this.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            if (Parent == parent)
            {
                IsFix = false;
                Parent = null;
                this.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    public void SetBossHP(EnemyBase parent, Character.AttackType attackType)
    {
        if (IsFix || attackType == Character.AttackType.Tick)
        {
            if (Parent == parent)
                SetValue();
        }
        else
        {
            if (Parent != null)
            {
                if (Parent == parent)
                    SetValue();
                else if ((int)parent.GetComponent<EnemyType>().enemyGrade >= (int)Parent.GetComponent<EnemyType>().enemyGrade)
                {
                    Parent = parent;
                    SetValue();
                    InitializeBossHPBar();
                }
            }
            else
            {
                Parent = parent;
                SetValue();
                InitializeBossHPBar();
            }
        }
    }

    private void SetValue()
    {
        BossHPMaxValue = Parent.MyStat.BaseMaxHealth;
        CurrenBossHPValue = Parent.MyStat.CurrentHealth;
        CurrentFill = CurrenBossHPValue / BossHPMaxValue;
        BossHPBarText.text = CurrenBossHPValue + "/" + BossHPMaxValue;
    }

    private void InitializeBossHPBar()
    {
        switch (Parent.GetComponent<EnemyType>().enemyGrade)
        {
            case EnemyType.EnemyGrade.Elite:
                // 이름 표시 예: 코볼드 근거리 LV.1 (정예)
                BossName.text = Parent.GetComponent<EnemyType>().EnemyName + " LV." + Parent.MyStat.Level + " (정예)";
                BossName.color = Color.yellow;
                CrownIcon.SetActive(false);
                break;

            case EnemyType.EnemyGrade.Guv:
                BossName.text = Parent.GetComponent<EnemyType>().EnemyName + " LV." + Parent.MyStat.Level + " (우두머리)";
                CrownIcon.SetActive(true);
                BossName.color = Color.red;
                break;
        }
        BossHPBarImage.fillAmount = CurrenBossHPValue / BossHPMaxValue;
    }
}
