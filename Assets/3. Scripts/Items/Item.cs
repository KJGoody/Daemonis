using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable , IDescribable
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string title;

    [SerializeField]
    private int stackSize;

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
        return title;
    }

}