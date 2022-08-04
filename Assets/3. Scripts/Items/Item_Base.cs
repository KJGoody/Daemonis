using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : IMoveable, IUseable
{
    public virtual ItemInfo_Base ItemInfo() { return null; }
    public ItemInfo_Base.Kinds Kind { get { return ItemInfo().Kind; } }
    public Sprite Icon { get { return ItemInfo().Icon; } }
    public string Name
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
            return string.Format("<color={0}>{1}</color>", color, ItemInfo().Name);
        }
    }
    public string Descript { get { return ItemInfo().Descript; } }
    public string Effect { get { return ItemInfo().Effect; } }
    public int LimitLevel { get { return ItemInfo().LimitLevel; } }
    public int Cost { get { return ItemInfo().Cost; } }

    public enum Quality { Normal, Advanced, Rare, Epic, Legendary, Relic }
    public Quality quality;
    public string QualityText
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

    public SlotScript MySlot;

    public string GetName() { return ItemInfo().Name; }
    public virtual void Use() { }
    public virtual void Remove() { }

    public virtual int GetPriorty() { return 0; }
}
