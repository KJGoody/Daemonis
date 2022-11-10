using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quester : NPC
{
    [SerializeField] private Text QuestMark;
    public static bool IsQuestTalk = false;

    private bool IsOverlap = false;

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

    public override Transform Select()
    {
        if (!IsOverlap)
        {
            IsOverlap = true;
            StartCoroutine(PreventOverlap());

            if (IsQuestTalk)
                DialogScript.Instance.OpenDialog("Quester");
            else
                DialogScript.Instance.OpenDialog("Quester", (int)DialogData.QuestStats.Ing);
        }

        return base.Select();
    }

    private IEnumerator PreventOverlap()
    {
        yield return new WaitForSeconds(0.1f);
        IsOverlap = false;
    }
}
