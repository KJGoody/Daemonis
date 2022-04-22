using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Quality { Normal, Advanced, Rare, Epic, Legendary, Relic }
public class ItemBase : IMoveable, IDescribable, IUseable
{
    public Item itemInfo;
    [SerializeField]
    private Quality quality;// ������ ���

    public Quality MyQuality
    {
        get
        {
            return quality;
        }
        set
        {
            quality = value;
        }
    }
    public Sprite MyIcon
    {
        get
        {
            return itemInfo.MyIcon;
        }
    }
    // �������� ��ø�� �� �ִ� ����
    // ��) �Ҹ� ������ ��� �Ѱ��� Slot�� ��������
    //     ��ø�Ǿ ������ �� ����.
    public int MyStackSize
    {
        get
        {
            return itemInfo.MyStackSize;
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

            return string.Format("<color={0}>{1}</color>", color, itemInfo.MyName);
        }

    }
    public string MyQualityText
    {
        get
        {
            string color = string.Empty;
            string str = "";
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
    }
    public Kinds GetKind
    {
        get
        {
            return itemInfo.GetKind;
        }
    }
    public SlotScript MySlot
    {
        get
        {
            return itemInfo.MySlot;
        }
        set
        {
            itemInfo.MySlot = value;
        }
    }
    public string MyDescript
    {
        get
        {
            return itemInfo.MyDescript;
        }
    }
    public string MyEffect
    {
        get
        {
            return itemInfo.MyEffect;
        }
    }
    public int MyLimitLevel
    {
        get
        {
            return itemInfo.MyLimitLevel;
        }
    }
    public void Remove()
    {
        if (MySlot != null)
        {
            //if (MySlot.MyCount == 0) // ���߿� �ٽ� �������� ���� ���� �����۽��� 0�϶� ���� �ϳ� ã�°�
                InventoryScript.MyInstance.FindUseSlot(this);
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

        return string.Format("<color={0}>{1}</color>", color, itemInfo.MyName);
    }
    public void Use()
    {
        HealthPotion healthPotion = itemInfo as HealthPotion;
        healthPotion.Use();
        Remove();
    }
    public string GetName() // �ϴ� useable������ �־�α���
    {
        return MyName;
    }

}
