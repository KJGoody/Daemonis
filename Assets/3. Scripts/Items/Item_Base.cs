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

    public Sprite MyIcon { get { return ItemInfo().MyIcon; } } // 아이템 아이콘 이미지

    public enum Quality { Normal, Advanced, Rare, Epic, Legendary, Relic }
    public Quality quality; // 아이템의 등급
    public string MyQualityText // 아이템 등급 표시
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

    public string GetName() { return MyName; }
    public string MyName // 아이템 이름 표시
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

            return string.Format("<color={0}>{1}</color>", color, ItemInfo().itemName);
        }
    }


    public ItemInfo_Base.Kinds GetKind { get { return ItemInfo().GetKind; } } // 아이템 종류
    public string MyDescript { get { return ItemInfo().descript; } } // 아이템 배경설명
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

        return string.Format("<color={0}>{1}</color>", color, ItemInfo().itemName);
    }
    public int MyLimitLevel { get { return ItemInfo().limitLevel; } } // 아이템 사용 제한 레벨
    public string MyEffect { get { return ItemInfo().effect; } } // 아이템 효과

    public SlotScript MySlot
    {
        get { return ItemInfo().slot; }
        set { ItemInfo().slot = value; }
    }

    public virtual void Use() { }
    public virtual void Remove() { }
}
