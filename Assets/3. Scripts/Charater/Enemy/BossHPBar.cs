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

    private void Update()
    {
        if (CurrentFill != BossHPBarImage.fillAmount)
            BossHPBarImage.fillAmount = Mathf.Lerp(BossHPBarImage.fillAmount, CurrentFill, Time.deltaTime);
    }

    public void BossHPBarSetActive(bool setactive, EnemyBase parent)
    {
        if (setactive)
        {
            SetBossHP(parent);
            this.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            Parent = null;
            this.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    private void SetBossHP(EnemyBase parent)
    {
        BossHPMaxValue = parent.MyStat.BaseMaxHealth;
        CurrenBossHPValue = parent.MyStat.CurrentHealth;
        CurrentFill = CurrenBossHPValue / BossHPMaxValue;
        BossHPBarText.text = CurrenBossHPValue + "/" + BossHPMaxValue;

        if (Parent == null)
        {
            Parent = parent;
            InitializeBossHPBar(parent.name);
        }
        else
        {
            if(Parent != parent)
            {
                Parent = parent;
                InitializeBossHPBar(parent.name);
            }
        }
    }

    private void InitializeBossHPBar(string bossname)
    {
        BossName.text = bossname;
        if (Parent is EnemyUnique)
        {
            BossName.color = Color.red;
            CrownIcon.SetActive(true);
        }
        else
        {
            BossName.color = Color.yellow;
            CrownIcon.SetActive(false);
        }
        BossHPBarImage.fillAmount = CurrenBossHPValue / BossHPMaxValue;
    }
}
