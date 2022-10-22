using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockStageButton : MonoBehaviour
{
    [SerializeField] private Button MyButton;
    [SerializeField] private string Stage;

    private void OnEnable()
    {
        MyButton.interactable = false;

        string[] stringSplit = Stage.Split('_');
        int chapterNum = int.Parse(stringSplit[1]);
        int stageNum = int.Parse(stringSplit[2]);

        // 해당 스테이지가 클리어되어 있다면
        if (GameManager.MyInstance.DATA.ClearStageNum[chapterNum - 1] != 0)
            // 클리어 스테이지가 자신의 스테이지 보다 높다면 해금
            if (GameManager.MyInstance.DATA.ClearStageNum[chapterNum - 1] + 1 >= stageNum)
            {
                MyButton.interactable = true;
                gameObject.SetActive(false);
            }

        // 챕터 2이상에서 첫번째 스테이지 일경우
        if(chapterNum >= 2 && stageNum == 1)
            // 이전 스테이지 클리어 스테이지 수가 10이상일 경우 스테이지를 해금한다.
            if(GameManager.MyInstance.DATA.ClearStageNum[(chapterNum - 1) - 1] >= 10)
            {
                MyButton.interactable = true;
                gameObject.SetActive(false);
            }
    }
}
