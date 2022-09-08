using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvadeGage : MonoBehaviour
{
    private static InvadeGage instance;
    public static InvadeGage Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InvadeGage>();
            return instance;
        }
    }

    [SerializeField] private Image FillImage;
    [SerializeField] private Text Percent;

    private float MaxValue;
    private float CurrentFill;
    private float currentValue;
    public float CurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = value;
            if (currentValue >= MaxValue) 
                currentValue = MaxValue;
            CurrentFill = currentValue / MaxValue;
        }
    }

    public void On()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GameManager.MyInstance.UnLoadSceneEvent += Off;
        MaxValue = 100;
    }

    public void Off()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GameManager.MyInstance.UnLoadSceneEvent -= Off;
    }

    private void Update()
    {
        if(CurrentFill != FillImage.fillAmount)
        {
            FillImage.fillAmount = Mathf.Lerp(FillImage.fillAmount, CurrentFill, Time.deltaTime * 2);
            Percent.text = Mathf.FloorToInt(CurrentFill * 100) + "%";
        }
    }
}