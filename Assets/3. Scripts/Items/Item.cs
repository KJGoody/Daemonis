using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private SlotScript slot;


    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }
    // 아이템이 중첩될 수 있는 개수
    // 예) 소모성 물약의 경우 한개의 Slot에 여러개가
    //     중첩되어서 보관될 수 있음.
    public int StackSize
    {
        get
        {
            return stackSize;
        }
    }

    protected SlotScript Slot
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
}