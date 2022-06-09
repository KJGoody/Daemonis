using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField]
    private Image StatBarImage;
    [SerializeField]
    private Text StatBarText;

    private float currentFill;
    [HideInInspector]
    public float StatBarMaxValue;

    private float currentValue;
    public float StatBarCurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = value;
            currentFill = currentValue / StatBarMaxValue;

            if (StatBarText != null)
                StatBarText.text = currentValue + " / " + StatBarMaxValue;
        }
    }

    void Update()
    {
        if (currentFill != StatBarImage.fillAmount)
            StatBarImage.fillAmount = Mathf.Lerp(StatBarImage.fillAmount, currentFill, Time.deltaTime * 2);
    }

    public void Initialize(float maxValue, float currentValue, bool FillAmountPass = false)
    {
        StatBarMaxValue = maxValue;
        StatBarCurrentValue = currentValue;
        if(!FillAmountPass)
            StatBarImage.fillAmount = 1;
    }

    public void SetMax(int maxValue)
    {
        StatBarMaxValue = maxValue;
        currentFill = currentValue / StatBarMaxValue;
        if(StatBarText != null)
            StatBarText.text = currentValue + " / " + StatBarMaxValue;
    }
}