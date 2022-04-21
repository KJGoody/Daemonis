using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    private Image BarImage;
    [SerializeField]
    private Text statText;
    [SerializeField]
    private float lerpSpeed;

    private float currentFill;
    [HideInInspector]
    public float MyMaxValue;

    private float currentValue;
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value > MyMaxValue)
                currentValue = MyMaxValue;
            else if (value < 0)
                currentValue = 0;
            else currentValue = value;

            currentFill = currentValue / MyMaxValue;

            if (statText != null)
                statText.text = currentValue + " / " + MyMaxValue;
        }
    }

    void Start()
    {
        BarImage = GetComponent<Image>();
    }

    void Update()
    {
        if (currentFill != BarImage.fillAmount)
        {
            BarImage.fillAmount = Mathf.Lerp(BarImage.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}