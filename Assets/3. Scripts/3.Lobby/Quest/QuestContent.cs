using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestContent : MonoBehaviour
{
    private QuestInfo Info;

    [SerializeField] private Text Title;
    [SerializeField] private Text Content;

    public void SetQuestContent(QuestInfo info)
    {
        Info = info;
        Title.text = info.Title;
        Content.text = info.Content;

        if (info.NPC_Start == "None")
            GameManager.MyInstance.DATA.Quest_Main_TalkStat[info.Index] = (int)DialogData.QuestStats.Done;
    }
}
