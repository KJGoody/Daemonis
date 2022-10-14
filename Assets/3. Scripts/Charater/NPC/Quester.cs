using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quester : NPC
{
    [SerializeField] private Text QuestMark;
    public static bool IsQuestTalk = false;

    private void Update()
    {
        if (QuestPanel.Instance.StartNPC == "Quester")
        {
            QuestMark.text = "!";
            IsQuestTalk = true;
            QuestMark.gameObject.SetActive(true);
        }
        else if (QuestPanel.Instance.DoneNPC == "Quester")
        {
            if (GameManager.MyInstance.DATA.Quest_Main_Stat == (int)DialogData.QuestStats.Done)
            {
                QuestMark.text = "?";
                IsQuestTalk = true;
                QuestMark.gameObject.SetActive(true);
            }
            else
            {
                IsQuestTalk = false;
                QuestMark.gameObject.SetActive(false);
            }
        }
        else
        {
            IsQuestTalk = false;
            QuestMark.gameObject.SetActive(false);
        }

    }

    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);

        if (target != null)
            ActiveButton.Instance.SetButton(ActiveButton.Role.QuesterButton, IsQuestTalk);
        else
            ActiveButton.Instance.ResetButton();
    }
}
