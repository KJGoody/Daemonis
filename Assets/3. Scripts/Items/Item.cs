using UnityEngine;

public enum Quality { Normal, Advanced, Rare, Epic , Legendary , Relic }
public enum Kinds { Common, Equipment, Potion }

public abstract class Item : ScriptableObject, IMoveable , IDescribable
{
    [SerializeField]
    private Sprite icon;    // ������ �̹���
    [SerializeField]
    private int stackSize;  // ��ø ����
    [SerializeField]
    private string itemName;   // ������ �̸�
    [SerializeField]
    private Quality quality;// ������ ���
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

            return string.Format("<color={0}>{1}</color>", color, itemName);
        }

        set
        {
            itemName = value;
        }
    }
    public string MyQuality
    {
        get
        {
            string color = string.Empty;
            string str="";
            switch (quality)
            {
                case Quality.Normal:
                    color = "#d6d6d6";
                    str = "�븻";
                    break;
                case Quality.Advanced:
                    color = "#00ff00ff";
                    str = "���";
                    break;
                case Quality.Rare:
                    color = "#0000ffff";
                    str = "���";
                    break;
                case Quality.Epic:
                    color = "#800080ff";
                    str = "����";
                    break;
            }

            return string.Format("<color={0}>{1}</color>", color, str);
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
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
    public virtual string GetDescription()
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

        return string.Format("<color={0}>{1}</color>", color, itemName);
    }


}