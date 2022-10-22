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
    [SerializeField] private GameObject MonsterIcon;
    [SerializeField] private Sprite[] Icon;
    [SerializeField] private Image BossHPBarImage;
    [SerializeField] private Text BossHPBarText;
    [SerializeField] private Text Buff;

    private EnemyBase Target;

    private float CurrentFill;
    private float BossHPMaxValue;
    private float CurrenBossHPValue;

    private bool IsFix;

    private void Update()
    {
        if(Target != null)
        {
            // 대상이 비활성화 또는 죽어 있다면 초기화한다.
            if (Target.gameObject.activeSelf == false || !Target.IsAlive)
            {
                Target = null;
                IsFix = false;
                BossHPBarSetActive(false);
            }
        }
        else
        {
            IsFix = false;
                BossHPBarSetActive(false);
        }

        // 몬스터 체력 게이지를 컨트롤한다.
        if (CurrentFill != BossHPBarImage.fillAmount)
            BossHPBarImage.fillAmount = Mathf.Lerp(BossHPBarImage.fillAmount, CurrentFill, Time.deltaTime);
    }

    public void BossHPBarSetActive(bool setactive)  // 보스 체력바를 보이게 또는 보이지 않게
    {
        if (setactive)
            GetComponent<CanvasGroup>().alpha = 1;
        else
            GetComponent<CanvasGroup>().alpha = 0;
    }

    // 보스 체력바를 보이게 보이지 않게, 대상의 정보, 현재 대상을 고정으로 표시할 것인가, 공격 타입
    public void BossHPBarSetActive(bool setactive, EnemyBase target, bool Fix = false, Character.AttackType attackType = Character.AttackType.Normal)
    {
        // 체력바 표시 대상 고정
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
        // 만약 고정이나 틱데미지일 경우 같은 대상일 경우에만 이미지 업데이트
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
                else if (target.GetComponent<EnemyType>().EnemeyGade.Length <= Target.GetComponent<EnemyType>().EnemeyGade.Length)
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
        switch (Target.GetComponent<EnemyType>().EnemeyGade)
        {
            case "Elite":
                // 이름 표시 예: 코볼드 근거리 LV.1 (정예)
                BossName.text = Target.GetComponent<EnemyType>().Name + " LV." + Target.MyStat.Level + " (정예)";
                MonsterIcon.SetActive(false);
                BossName.color = Color.blue;
                break;

            case "Guv":
                BossName.text = Target.GetComponent<EnemyType>().Name + " LV." + Target.MyStat.Level + " (우두머리)";
                MonsterIcon.GetComponent<Image>().sprite = Icon[0]; 
                MonsterIcon.SetActive(true); 
                BossName.color = Color.yellow;
                break;

            case "Boss":
                BossName.text = Target.GetComponent<EnemyType>().Name + " LV." + Target.MyStat.Level + " (보스)";
                MonsterIcon.GetComponent<Image>().sprite = Icon[1];
                MonsterIcon.SetActive(true);
                BossName.color = Color.red;
                break;
        }
        BossHPBarImage.fillAmount = CurrenBossHPValue / BossHPMaxValue;

        string BuffList = "";
        for(int i = 0; i < Target.OnBuff.Count; i++)
        {
            if(Target.OnBuff[i].BuffText != "")
                BuffList += Target.OnBuff[i].BuffText + ", ";
        }
        Buff.text = BuffList.Substring(0, BuffList.Length -2);
    }
}
