using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : IMoveable, IDescribable, IUseable
{
    public virtual ItemInfo_Base ItemInfo() { return null; }
    public ItemInfo_Base.Kinds Kind { get { return ItemInfo().Kind; } }
    public Sprite Icon { get { return ItemInfo().Icon; } }
    // 아이템의 이름 표시
    public string Name
    {
        get
        {
            string color = string.Empty;
            #region 아이템 이름 색상 설정
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
    public int LimitLevel { get { return ItemInfo().LimitLevel; } } // 아이템 사용 제한 레벨
    public int Cost { get { return ItemInfo().Cost; } }

    public enum Quality { Normal, Advanced, Rare, Epic, Legendary, Relic }
    // 아이템의 등급
    public Quality quality;
    public string MyQualityText
    {
        get
        {
            string color = string.Empty;
            string str = "";
            #region 아이템 등급 색상 설정
            switch (quality)
            {
                case Quality.Normal:
                    color = "#d6d6d6";
                    str = "노말";
                    break;
                case Quality.Advanced:
                    color = "#00ff00ff";
                    str = "고급";
                    break;
                case Quality.Rare:
                    color = "#0000ffff";
                    str = "희귀";
                    break;
                case Quality.Epic:
                    color = "#800080ff";
                    str = "영웅";
                    break;
                case Quality.Legendary:
                    color = "#ffff00ff";
                    str = "전설";
                    break;
                case Quality.Relic:
                    color = "#ff6600ff";
                    str = "유물";
                    break;
            }
            #endregion

            return string.Format("<color={0}>{1}</color>", color, str);
        }
    }

    // 아이템의 종류
    public virtual string GetDescription()
    {
        string color = string.Empty;
        #region 아이템 설명 색상 지정
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

        return string.Format("<color={0}>{1}</color>", color, ItemInfo().Name);
    }

    public SlotScript MySlot;

    public virtual int GetPriorty() { return 0; }

    public string GetName() { return ItemInfo().Name; }
    public virtual void Use() { }
    public virtual void Remove() { }
}
