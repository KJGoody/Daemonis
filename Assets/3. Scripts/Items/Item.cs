using UnityEngine;

public enum Quality { Normal, Advanced, Rare, Epic , Legendary , Relic }

public abstract class Item : ScriptableObject, IMoveable , IDescribable
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int stackSize;
    [SerializeField]
    private string title;
    [SerializeField]
    private Quality quality;

    private SlotScript slot;


    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }
    // 아이템이 중첩될 수 있는 개수
    // 예) 소모성 물약의 경우 한개의 Slot에 여러개가
    //     중첩되어서 보관될 수 있음.
    public int MyStackSize
    {
        get
        {
            return stackSize;
        }
    }

    public SlotScript MySlot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }
    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
    public string GetDescription()
    {
        string color = string.Empty;

        switch (quality)
        {
            case Quality.Normal:
                color = "#d6d6d6";
                break;
            case Quality.Advanced:
                color = "#00ff00ff";
                break;
            case Quality.Rare:
                color = "#0000ffff";
                break;
            case Quality.Epic:
                color = "#800080ff";
                break;
        }

        return string.Format("<color={0}>{1}</color>", color, title);
    }


}