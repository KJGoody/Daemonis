using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInfo
{
    public int Index;
    public enum Types { Auto, Talk }
    public Types Type;
    public string NPC_Start;
    public string NPC_Done;
    public string Title;
    public string Content;
    public enum GoalTypes { Talk, Stage, Kill }
    public GoalTypes GoalType;
    public string Goal;
    public string Rewards;
}
