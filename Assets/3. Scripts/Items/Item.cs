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
    // �������� ��ø�� �� �ִ� ����
    // ��) �Ҹ� ������ ��� �Ѱ��� Slot�� ��������
    //     ��ø�Ǿ ������ �� ����.
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