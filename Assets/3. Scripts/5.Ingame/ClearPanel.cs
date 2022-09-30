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

    public void ClearGame()
    {

        string[] stringSplit = GameManager.MyInstance.CurrentStageID.Split('_');
        if(GameManager.MyInstance.DATA.ClearStageNum[int.Parse(stringSplit[1]) - 1] < int.Parse(stringSplit[2]))
            GameManager.MyInstance.DATA.ClearStageNum[int.Parse(stringSplit[1]) - 1] = int.Parse(stringSplit[2]);
        GameManager.MyInstance.SaveData();
        GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(10f);
        GetComponent<CanvasGroup>().alpha = 0;
    }
}
