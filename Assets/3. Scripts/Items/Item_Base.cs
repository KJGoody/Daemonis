using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : IMoveable, IUseable, IItem
{
    public void SetInfo() { }

    //-- IItem --
    public virtual string ID { get; }
    public virtual ItemInfo_Base.Kinds Kind { get; }
    public virtual Sprite Icon { get; }
    //-- IUseable --
    public virtual string Name { get; }
    public virtual string Descript { get; }
    public virtual string Effect { get; }
    public virtual int LimitLevel { get; }
    public virtual int Cost { get; }
    //-- IItem --

    //-- IMoveable --
    public virtual string GetName() { return null; }

    //-- IUseable --
    public virtual void Use() { }

    public virtual void Remove() { }
    public virtual int GetPriorty() { return 0; }

    //-- Item_Base --
    public enum Qualitys { Normal, Advanced, Rare, Epic, Legendary, Relic }
    public Qualitys Quality;
    public string QualityText
    {
        get
        {
            string color = string.Empty;
            string str = "";
            #region ������ ��� ���� ����
            switch (Quality)
            {
                case Qualitys.Normal:
                    color = "#d6d6d6";
                    str = "�븻";
                    break;
                case Qualitys.Advanced:
                    color = "#00ff00ff";
                    str = "���";
                    break;
                case Qualitys.Rare:
                    color = "#0000ffff";
                    str = "���";
                    break;
                case Qualitys.Epic:
                    color = "#800080ff";
                    str = "����";
                    break;
                case Qualitys.Legendary:
                    color = "#ffff00ff";
                    str = "����";
                    break;
                case Qualitys.Relic:
                    color = "#ff6600ff";
                    str = "����";
                    break;
            }
            #endregion

            return string.Format("<color={0}>{1}</color>", color, str);
        }
    }

    public SlotScript MySlot;
}
