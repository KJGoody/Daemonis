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

    [HideInInspector] public bool SetTime = false;
    [HideInInspector] public float TotalTime;
    [HideInInspector] public bool IsBossTime = false;
    [HideInInspector] public float BossTime;

    public void On(int MaxValue)
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GameManager.MyInstance.UnLoadSceneEvent += Off;
        this.MaxValue = MaxValue;
        SetTime = true;
    }

    public void Off()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GameManager.MyInstance.UnLoadSceneEvent -= Off;
        CurrentValue = 0;
        SetTime = false;
        TotalTime = 0;
        IsBossTime = false;
        BossTime = 0;
    }

    private void Update()
    {
        if(CurrentFill != FillImage.fillAmount)
        {
            FillImage.fillAmount = Mathf.Lerp(FillImage.fillAmount, CurrentFill, Time.deltaTime * 2);
            Percent.text = Mathf.FloorToInt(CurrentFill * 100) + "%";
        }

        if(FillImage.fillAmount > 0.99f && !IsBossTime)
            IsBossTime = true;

        if (SetTime)
            TotalTime += Time.deltaTime;
        if (IsBossTime)
            BossTime += Time.deltaTime;
    }
}
