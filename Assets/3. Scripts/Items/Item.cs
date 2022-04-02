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
    public string GetDescription()
    {
        return title;
    }

}