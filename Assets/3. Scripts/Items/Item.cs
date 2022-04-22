using UnityEngine;

public enum Kinds { Common, Equipment, Potion }

public abstract class Item : ScriptableObject
{
    [SerializeField]
    private Sprite icon;    // ������ �̹���
    [SerializeField]
    private int stackSize;  // ��ø ����
    [SerializeField]
    private string itemName;   // ������ �̸�
    //[SerializeField]
    //private Quality quality;// ������ ���
    [SerializeField]
    private string descript;// ������ ���� (��漳��������)
    [SerializeField]
    private string effect;  // ������ ȿ�� ����
    [SerializeField]
    private int limitLevel; // ������ ���� ����
    [SerializeField]
    private Kinds kind;// ������ ����
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
    public string MyName
    {
        get
        {

            return itemName;
        }

        set
        {
            itemName = value;
        }
    }

    public Kinds GetKind
    {
        get
        {
            Kinds myKind = Kinds.Common;
            switch (kind)
            {
                case Kinds.Equipment:
                    myKind = Kinds.Equipment;
                    break;
                case Kinds.Potion:
                    myKind = Kinds.Potion;
                    break;
            }
            return myKind;
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
    public string MyDescript
    {
        get
        {
            return descript;
        }

        set
        {
            descript = value;
        }
    }
    public string MyEffect
    {
        get
        {
            return effect;
        }

        set
        {
            effect = value;
        }
    }
    public int MyLimitLevel
    {
        get
        {
            return limitLevel;
        }

        set
        {
            limitLevel = value;
        }
    }
    public void Remove()
    {
        //if (MySlot != null)
        //{
        //    if(MySlot.MyCount == 0)
        //       InventoryScript.MyInstance.FindUseSlot(this);
        //    MySlot.RemoveItem(this);
        //}
    }
    public virtual string GetDescription()
    {

        return itemName;
    }

}