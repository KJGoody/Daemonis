using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    private Image StatBarImage;
    [SerializeField]
    private Text StatBarText;
    [SerializeField]
    private float lerpSpeed;

    private float currentFill;
    [HideInInspector]
    public float StatBarMaxValue;

    private float currentValue;
    public float StatBarCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value > StatBarMaxValue)
                currentValue = StatBarMaxValue;
            else if (value < 0) 
                currentValue = 0;
            else currentValue = value;

            currentFill = currentValue / StatBarMaxValue;

            if (StatBarText != null)
                StatBarText.text = currentValue + " / " + StatBarMaxValue;
        }
    }

    void Start()
    {
        StatBarImage = GetComponent<Image>();
    }

    void Update()
    {

        if (currentFill != StatBarImage.fillAmount)
        {
            StatBarImage.fillAmount = Mathf.Lerp(StatBarImage.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float maxValue, float currentValue)
    {
        StatBarMaxValue = maxValue;
        StatBarCurrentValue = currentValue;
    }
}