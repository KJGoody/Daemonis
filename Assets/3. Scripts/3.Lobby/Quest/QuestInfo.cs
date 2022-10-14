public class QuestInfo
{
    public int Index;
    public enum Types { Auto, Talk }
    public Types Type;
    public string NPC_Start;
    public string NPC_Done;
    public string Title;
    public string Content;
    public string Content_Done;
    public enum GoalTypes { None, Stage, Kill }
    public GoalTypes GoalType;
    public string Goal;
    public string[] Goal_Kill { get { return Goal.Split('/'); } }
    public string Rewards;
}
