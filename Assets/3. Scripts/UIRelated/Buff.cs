using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Buff : MonoBehaviour
{
    [SerializeField]
    private Image BuffFillImage;
    public enum BuffType { Standard, Stack, Toggle, Passive }
    [SerializeField]
    private BuffType buffType;
    public string BuffName;
    [SerializeField]
    private float Duration;
    private float currentTime;

    private BuffManager MyManager;
    private Puff PuffObject;
    [SerializeField]
    private TextMeshProUGUI StackText;
    private Color TextColor;

    private Character Target;
    private bool InTargetGroup = false;
    [HideInInspector]
    public int BuffStack = 1;


    private void Awake()
    {
        if (buffType.Equals(BuffType.Passive))
        {
            BuffFillImage.gameObject.SetActive(false);
        }
        else
        {
            currentTime = Duration;

            if (buffType == BuffType.Stack)
            {
                TextColor = StackText.color;
                TextColor.a = 1;
                StackText.color = TextColor;
                StackText.text = BuffStack.ToString();
            }
        }
    }

    private void Update()
    {
        if (!Target.IsAlive || !Target.gameObject.activeSelf)
            DeActivationBuff();
    }

    public void ResetBuff() // ���� ����
    {
        currentTime = Duration;
        BuffFillImage.fillAmount = 1;
        if (buffType == BuffType.Stack)
        {
            if (BuffStack < 5)
                BuffStack++;
            StackText.text = BuffStack.ToString();
        }
    }

    public void ExecuteBuff(BuffManager buffManager, Character target)
    {
        MyManager = buffManager;
        Target = target;
        Target.OnBuff.Add(this);

        if (!buffType.Equals(BuffType.Passive))
            StartCoroutine(ActivationBuff());

        switch (BuffName)
        {
            case "Skill_Fire_02_Debuff":
                StartCoroutine(Skill_Fire_02_Debuff());
                break;

            case "HealPotion_Buff":
                StartCoroutine(HealPotion_Buff());
                break;
        }
    }

    private IEnumerator ActivationBuff()
    {
        while (currentTime > 0)
        {
            currentTime -= 0.1f;
            BuffFillImage.fillAmount = currentTime / Duration;
            yield return new WaitForSeconds(0.1f);
        }
        BuffFillImage.fillAmount = 0;
        DeActivationBuff();
    }

    public void DeActivationBuff()
    {
        Target.OnBuff.Remove(this);
        MyManager.BuffList.Remove(gameObject);
        if (PuffObject != null)
            PuffPool.Instance.ReturnObject(PuffObject, PuffPool.PuffPrefabsName.Fire);
        if (InTargetGroup)
            Player.MyInstance.RemoveTarget(BuffName, Target.transform);
        Destroy(gameObject);
    }

    private IEnumerator Skill_Fire_02_Debuff()
    {
        Player.MyInstance.AddTarget(BuffName, Target.transform);
        InTargetGroup = true;
        PuffObject = PuffPool.Instance.GetObject(PuffPool.PuffPrefabsName.Fire);
        PuffObject.PositioningPuff(Target.transform);

        float WaitForSconds = 0.5f;
        int TickDamage = Player.MyInstance.MyStat.BaseAttack / 25;

        while (true)
        {
            if (Target.IsAlive)
            {
                if (ChanceMaker.GetThisChanceResult_Percentage(Player.MyInstance.MyStat.CriticalPercent))
                    Target.TakeDamage(Character.DamageType.Masic, 1, TickDamage * BuffStack, Target.MyStat.Level, Vector2.zero, NewTextPool.NewTextPrefabsName.Critical, Character.AttackType.Tick);
                else
                    Target.TakeDamage(Character.DamageType.Masic, 1, TickDamage * BuffStack, Target.MyStat.Level, Vector2.zero, NewTextPool.NewTextPrefabsName.Enemy, Character.AttackType.Tick);
            }
            yield return new WaitForSeconds(WaitForSconds);
        }
    }

    private IEnumerator HealPotion_Buff()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Player.MyInstance.MyStat.CurrentHealth += 20;
        }
    }
}
