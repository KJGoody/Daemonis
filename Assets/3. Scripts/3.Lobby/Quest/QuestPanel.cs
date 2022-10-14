using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    private static QuestPanel instance;
    public static QuestPanel Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<QuestPanel>();
            return instance;
        }
    }

    public Transform Content;
    private List<QuestContent> Quests = new List<QuestContent>();

    [HideInInspector] public string StartNPC = null;
    private QuestInfo Data;
    [HideInInspector] public string DoneNPC = null;

    private void Start()
    {
        UpdateQuestPanel();
    }

    public void UpdateQuestPanel()
    {
        QuestInfo data = DataTableManager.Instance.GetQuestInfo(GameManager.MyInstance.DATA.Quest_Main);
        if(data != null)
        {
            QuestContent quest;
            switch (data.Type)
            {
                case QuestInfo.Types.Auto:
                    quest = Instantiate(Resources.Load<GameObject>("Prefabs/QuestContent"), Content).GetComponent<QuestContent>();
                    Quests.Add(quest);
                    quest.SetQuestContent(data);
                    DoneNPC = data.NPC_Done;
                    break;

                case QuestInfo.Types.Talk:
                    switch (GameManager.MyInstance.DATA.Quest_Main_Stat)
                    {
                        case (int)DialogData.QuestStats.Start:
                            StartNPC = data.NPC_Start;
                            Data = data;
                            break;

                        case (int)DialogData.QuestStats.Ing:
                            quest = Instantiate(Resources.Load<GameObject>("Prefabs/QuestContent"), Content).GetComponent<QuestContent>();
                            Quests.Add(quest);
                            quest.SetQuestContent(data);
                            DoneNPC = data.NPC_Done;
                            break;

                        case (int)DialogData.QuestStats.Done:
                            quest = Instantiate(Resources.Load<GameObject>("Prefabs/QuestContent"), Content).GetComponent<QuestContent>();
                            Quests.Add(quest);
                            quest.SetQuestContent(data);
                            DoneNPC = data.NPC_Done;
                            quest.SetDone();
                            break;
                    }
                    break;
            }
        }
    }

    public void CheckQuestGoal(QuestInfo.GoalTypes type, string goal)
    {
        foreach(QuestContent quest in Quests)
        {
            quest.CheckGoal(type, goal);
        }
    }
    
    public void TalkDone(string NPCname)
    {
        bool isStartTalk = false;

        if(StartNPC == NPCname)
        {
            StartNPC = null;
            isStartTalk = true;
            GameManager.MyInstance.DATA.Quest_Main_Stat++;

            QuestContent quest = Instantiate(Resources.Load<GameObject>("Prefabs/QuestContent"), Content).GetComponent<QuestContent>();
            Quests.Add(quest);
            quest.SetQuestContent(Data);
            DoneNPC = Data.NPC_Done;
        }

        if (!isStartTalk)
        {
            for (int i = 0; i < Quests.Count; i++)
            {
                if (Quests[i].QuestDone(NPCname))
                {
                    Quests.Remove(Quests[i]);
                    break;
                }

            }
        }
    }
}
