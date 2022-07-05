using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : IMoveable, IDescribable, IUseable
{
    public ItemInfo_Base itemInfo;
    public virtual ItemInfo_Base ItemInfo()
    {
        ItemInfo_Base itemInfo = this.itemInfo;
        return itemInfo;
    }

    public Sprite MyIcon { get { return ItemInfo().MyIcon; } } // ������ ������ �̹���

    public enum Quality { Normal, Advanced, Rare, Epic, Legendary, Relic }
    public Quality quality; // �������� ���
    public string MyQualityText // ������ ��� ǥ��
    {
        get
        {
            string color = string.Empty;
            string str = "";
            #region ������ ��� ���� ����
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
                case Quality.Legendary:
                    color = "#ffff00ff";
                    str = "����";
                    break;
                case Quality.Relic:
                    color = "#ff6600ff";
                    str = "����";
                    break;
            }
            #endregion

            return string.Format("<color={0}>{1}</color>", color, str);
        }
    }

    public string GetName() { return MyName; }
    public string MyName // ������ �̸� ǥ��
    {
        get
        {
            string color = string.Empty;
            #region ������ �̸� ���� ����
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
                case Quality.Legendary:
                    color = "#ffff00ff";
                    break;
                case Quality.Relic:
                    color = "#ff6600ff";
                    break;
            }
            #endregion

            return string.Format("<color={0}>{1}</color>", color, ItemInfo().itemName);
        }
    }


    public ItemInfo_Base.Kinds GetKind { get { return ItemInfo().GetKind; } } // ������ ����
    public string MyDescript { get { return ItemInfo().descript; } } // ������ ��漳��
    public virtual string GetDescription()
    {
        string color = string.Empty;
        #region ������ ���� ���� ����
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
        #endregion

        return string.Format("<color={0}>{1}</color>", color, ItemInfo().itemName);
    }
    public int MyLimitLevel { get { return ItemInfo().limitLevel; } } // ������ ��� ���� ����
    public string MyEffect { get { return ItemInfo().effect; } } // ������ ȿ��

    public SlotScript MySlot
    {
        get { return ItemInfo().slot; }
        set { ItemInfo().slot = value; }
    }

    public virtual void Use() { }
    public virtual void Remove() { }
}
