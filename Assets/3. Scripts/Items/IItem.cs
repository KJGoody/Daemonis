using UnityEngine;

public interface IItem
{
    public string ID { get; }
    public ItemInfo_Base.Kinds Kind { get; }
    public Sprite Icon { get; }
    public string Name { get; }
    public string Descript { get; }
    public string Effect { get; }
    public int LimitLevel { get; }
    public int Cost { get; }
}
