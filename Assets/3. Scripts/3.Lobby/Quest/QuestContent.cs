using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestContent : MonoBehaviour
{
    private QuestInfo Info;

    [SerializeField] private Text Title;
    [SerializeField] private Text Content;

    private bool IsClear = false;

    private void Update()
    {
        if (Info != null && GameManager.MyInstance.DATA.Quest_Main_Stat == (int)DialogData.QuestStats.Done)
            IsClear = true;
    }

    public void SetQuestContent(QuestInfo info)
    {
        Info = info;
        Title.text = info.Title;

        switch (info.GoalType)
        {
            case QuestInfo.GoalTypes.None:
                GameManager.MyInstance.DATA.Quest_Main_Stat = (int)DialogData.QuestStats.Done;
                IsClear = true;
                Content.text = info.Content_Done;
                break;

            case QuestInfo.GoalTypes.Stage:
                Content.text = info.Content;
                break;

            case QuestInfo.GoalTypes.Kill:
                if (GameManager.MyInstance.DATA.Quest_Main_Goal == "")
                    GameManager.MyInstance.DATA.Quest_Main_Goal = "0";
                Content.text = Info.Content + " (" + GameManager.MyInstance.DATA.Quest_Main_Goal + "/" + Info.Goal_Kill[1] + ")";
                break;
        }
    }

    public void CheckGoal(QuestInfo.GoalTypes type, string goal)
    {
        if (Info.GoalType != type) return;

        switch (type)
        {
            case QuestInfo.GoalTypes.Stage:
                if(Info.Goal == goal)
                {
                    GameManager.MyInstance.DATA.Quest_Main_Stat = (int)DialogData.QuestStats.Done;
                    IsClear = true;
                    Content.text = Info.Content_Done;
                }
                break;

            case QuestInfo.GoalTypes.Kill:
                string[] target = Info.Goal_Kill[0].Split('_');
                string[] check = goal.Split('_');
                if (target[1] == check[1] && target[2] == check[2])
                {
                    if (GameManager.MyInstance.DATA.Quest_Main_Goal == "")
                        GameManager.MyInstance.DATA.Quest_Main_Goal = "0";
                    else
                        GameManager.MyInstance.DATA.Quest_Main_Goal = (int.Parse(GameManager.MyInstance.DATA.Quest_Main_Goal) + 1).ToString();

                    if (int.Parse(GameManager.MyInstance.DATA.Quest_Main_Goal) >= int.Parse(Info.Goal_Kill[1]))
                    {
                        GameManager.MyInstance.DATA.Quest_Main_Stat = (int)DialogData.QuestStats.Done;
                        IsClear = true;
                        Content.text = Info.Content_Done;
                    }
                    else
                        Content.text = Info.Content + " (" + GameManager.MyInstance.DATA.Quest_Main_Goal + "/" + Info.Goal_Kill[1] + ")";
                }
                break;
        }
    }

    public void SetDone()
    {
        IsClear = true;
        Content.text = Info.Content_Done;
    }

    public bool QuestDone(string NPCname)
    {
        if (IsClear)
        {
            if (Info.NPC_Done == NPCname)
            {
                GameManager.MyInstance.DATA.Quest_Main++;
                GameManager.MyInstance.DATA.Quest_Main_Stat = 0;
                GameManager.MyInstance.DATA.Quest_Main_Goal = "";
                QuestPanel.Instance.DoneNPC = null;
                QuestPanel.Instance.UpdateQuestPanel();
                GiveRewards();
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }

    private void GiveRewards()
    {
        if (Info.Rewards != "None")
        {
            string[] RewardsSplit = Info.Rewards.Split('/');
            for (int i = 0; i < RewardsSplit.Length; i++)
            {
                string[] Reward = RewardsSplit[i].Split('_');
                switch (Reward[0])
                {
                    case "Gold":
                        GameManager.MyInstance.DATA.Gold += int.Parse(Reward[1]);
                        break;

                    case "EXP":
                        Player.MyInstance.MyStat.CurrentEXP += int.Parse(Reward[1]);
                        break;

                    case "E":
                        List<ItemInfo_Equipment> array = DataTableManager.Instance.GetInfo_Equipments(Player.MyInstance.MyStat.Level);
                        for (int j = 0; j < int.Parse(Reward[1]); j++)
                        {
                            ItemCart dropitem = Instantiate(Resources.Load<GameObject>("Prefabs/P_DropItem"),
                                Player.MyInstance.transform.position + ((Vector3)Random.insideUnitCircle * 0.5f),
                                Quaternion.identity).GetComponent<ItemCart>();

                            dropitem.SetItem_Equipment(array[Random.Range(0, array.Count)], DataTableManager.Instance.GetQuality(Player.MyInstance.MyStat.Level));
                        }
                        break;
                }
            }
        }
    }
}
