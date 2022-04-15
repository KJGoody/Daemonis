using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Stat : MonoBehaviour
{
    private Image content;

    [SerializeField]
    private Text statText;
    [SerializeField]
    private float lerpSpeed;

    private float currentFill;
    public float MyMaxValue { get; set; }

    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value > MyMaxValue) currentValue = MyMaxValue;
            else if (value < 0) currentValue = 0;
            else currentValue = value;

            currentFill = currentValue / MyMaxValue;
            if (statText != null) statText.text = currentValue + " / " + MyMaxValue;
        }
    }

    private float currentValue;

    void Start()
    {
        content = GetComponent<Image>();
    }

    void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}