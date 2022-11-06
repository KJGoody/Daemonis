using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanel : MonoBehaviour
{
    private static ClearPanel instance;
    public static ClearPanel Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ClearPanel>();
            return instance;
        }
    }

    [SerializeField] private Text ComboText;
    [SerializeField] private Text KillText;
    [SerializeField] private Text TotalTimeText;
    [SerializeField] private Text BossTimeText;

    private Coroutine CurrentCoroutine;

    [HideInInspector] public int KillCount = 0;

    public void ClearGame()
    {
        QuestPanel.Instance.CheckQuestGoal(QuestInfo.GoalTypes.Stage, GameManager.MyInstance.CurrentStageID);

        string[] stringSplit = GameManager.MyInstance.CurrentStageID.Split('_');
        if (GameManager.MyInstance.DATA.ClearStageNum[int.Parse(stringSplit[1]) - 1] < int.Parse(stringSplit[2]))
            GameManager.MyInstance.DATA.ClearStageNum[int.Parse(stringSplit[1]) - 1] = int.Parse(stringSplit[2]);

        ComboText.text = "Combo: " + ComboManager.Instance.BestCombo.ToString();
        ComboManager.Instance.BestCombo = 0;
        KillText.text = "TotalKill: " + KillCount.ToString();
        KillCount = 0;
        TotalTimeText.text = "TotalTime: " + Mathf.FloorToInt(InvadeGage.Instance.TotalTime).ToString();
        BossTimeText.text = "BossTime: " + Mathf.FloorToInt(InvadeGage.Instance.BossTime).ToString();
        GameManager.MyInstance.SaveData();
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        CurrentCoroutine = StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(10f);
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void Update()
    {
        if(GameManager.MyInstance.CurrnetSceneName != "5.IngameMap")
        {
            if(CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);
            GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void _Close()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
