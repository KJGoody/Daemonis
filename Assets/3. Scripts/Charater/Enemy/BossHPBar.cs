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

    [SerializeField]
    private Text BossName;
    [SerializeField]
    private GameObject CrownIcon;
    [SerializeField]
    private Image BossHPBarImage;
    [SerializeField]
    private Text BossHPBarText;

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

    public void BossHPBarSetActive(bool setactive, EnemyBase parent, bool Fix = false)
    {
        if (Fix)
        {
            if (setactive) IsFix = true;
            else IsFix = false;
        }

        if (setactive)
        {
            SetBossHP(parent);
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

    public void SetBossHP(EnemyBase parent)
    {
        if (Parent != null)
        {
            if (Parent == parent)
                SetValue();
            else if ((int)parent.GetComponent<EnemyType>().enemyGrade > (int)Parent.GetComponent<EnemyType>().enemyGrade)
            {
                if (IsFix)
                {
                    Parent = parent;
                    SetValue();
                    InitializeBossHPBar();
                }
            }
        }
        else
        {
            if (!IsFix)
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
                BossName.text = Parent.GetComponent<EnemyType>().EnemyName + "(정예)";
                BossName.color = Color.yellow;
                CrownIcon.SetActive(false);
                break;

            case EnemyType.EnemyGrade.Guv:
                BossName.text = Parent.GetComponent<EnemyType>().EnemyName + "(우두머리)";
                CrownIcon.SetActive(true);
                BossName.color = Color.red;
                break;
        }
        BossHPBarImage.fillAmount = CurrenBossHPValue / BossHPMaxValue;
    }
}
