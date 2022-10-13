using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    public Transform Content;

    private void Start()
    {
        UpdateQuestPanel();
    }

    public void UpdateQuestPanel()
    {
        QuestInfo data = DataTableManager.Instance.GetQuestInfo(GameManager.MyInstance.DATA.Quest_Main);
        if(data != null)
        {
            if(data.Type == QuestInfo.Types.Auto)
            {
                QuestContent quest = Instantiate(Resources.Load<GameObject>("Prefabs/QuestContent"), Content).GetComponent<QuestContent>();
                quest.SetQuestContent(data);
            }
        }
    }
}
