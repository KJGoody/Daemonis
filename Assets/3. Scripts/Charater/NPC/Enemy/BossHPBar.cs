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

    private EnemyBase Target;

    private float CurrentFill;
    private float BossHPMaxValue;
    private float CurrenBossHPValue;

    private bool IsFix;

    private void Update()
    {
        if(Target != null && Target.gameObject.activeSelf == false)
        {
            Target = null;
            IsFix = false;
        }
        if (CurrentFill != BossHPBarImage.fillAmount)
            BossHPBarImage.fillAmount = Mathf.Lerp(BossHPBarImage.fillAmount, CurrentFill, Time.deltaTime);
    }

    public void BossHPBarSetActive(bool setactive)
    {
        if (setactive)
            GetComponent<CanvasGroup>().alpha = 1;
        else
            GetComponent<CanvasGroup>().alpha = 0;
    }

    public void BossHPBarSetActive(bool setactive, EnemyBase target, bool Fix = false, Character.AttackType attackType = Character.AttackType.Normal)
    {
        // ü�¹� ǥ�� ��� ����
        if (Fix)
            if (setactive)
            {
                Target = target;
                SetValue();
                InitializeBossHPBar();
                IsFix = true;
            }
            else
            {
                Target = null;
                IsFix = false;
            }

        if (setactive)
        {
            SetBossHP(target, attackType);
            GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            if (Target == target)
            {
                IsFix = false;
                Target = null;
                GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    public void SetBossHP(EnemyBase target, Character.AttackType attackType)
    {
        if (IsFix || attackType == Character.AttackType.Tick)
        {
            if (Target == target)
                SetValue();
        }
        else
        {
            if (Target != null)
            {
                if (Target == target)
                    SetValue();
                else if ((int)target.GetComponent<EnemyType>().enemyGrade >= (int)Target.GetComponent<EnemyType>().enemyGrade)
                {
                    Target = target;
                    SetValue();
                    InitializeBossHPBar();
                }
            }
            else
            {
                Target = target;
                SetValue();
                InitializeBossHPBar();
            }
        }
    }

    private void SetValue()
    {
        BossHPMaxValue = Target.MyStat.BaseMaxHealth;
        CurrenBossHPValue = Target.MyStat.CurrentHealth;
        CurrentFill = CurrenBossHPValue / BossHPMaxValue;
        BossHPBarText.text = CurrenBossHPValue + "/" + BossHPMaxValue;
    }

    private void InitializeBossHPBar()
    {
        switch (Target.GetComponent<EnemyType>().enemyGrade)
        {
            case EnemyType.EnemyGrade.Elite:
                // �̸� ǥ�� ��: �ں��� �ٰŸ� LV.1 (����)
                BossName.text = Target.GetComponent<EnemyType>().EnemyName + " LV." + Target.MyStat.Level + " (����)";
                BossName.color = Color.yellow;
                CrownIcon.SetActive(false);
                break;

            case EnemyType.EnemyGrade.Guv:
                BossName.text = Target.GetComponent<EnemyType>().EnemyName + " LV." + Target.MyStat.Level + " (��θӸ�)";
                CrownIcon.SetActive(true);
                BossName.color = Color.red;
                break;
        }
        BossHPBarImage.fillAmount = CurrenBossHPValue / BossHPMaxValue;
    }
}
