using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    public Image BuffFillImage;
    private Character Target;

    public string BuffName;
    public float Duration;
    private float currentTime;

    private void Awake()
    {
        currentTime = Duration;
    }

    public void ResetBuff()
    {
        currentTime = Duration;
        BuffFillImage.fillAmount = 1;
    }

    public void ExecuteBuff(Character target)
    {
        Target = target;
        Target.onBuff.Add(this);
        StartCoroutine(ActivationBuff());
    }

    IEnumerator ActivationBuff()
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

    void DeActivationBuff()
    {
        Target.onBuff.Remove(this);
        BuffManager.myInstance.BuffList.Remove(gameObject);
        Destroy(gameObject);
    }
}
