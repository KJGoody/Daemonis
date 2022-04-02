using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField]
    private Sprite icon;


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
    // �������� ��ø�� �� �ִ� ����
    // ��) �Ҹ� ������ ��� �Ѱ��� Slot�� ��������
    //     ��ø�Ǿ ������ �� ����.
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

}