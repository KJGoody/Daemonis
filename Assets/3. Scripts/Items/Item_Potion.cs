using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Potion : Item_Consumable
{
    private ItemInfo_Potion Info;
    public ItemInfo_Potion GetInfo() { return Info; }
    public void SetInfo(ItemInfo_Potion Info) 
    {
        this.Info = Info;
        IsSetInfo = true;
    }

    //-- IItem --
    public override string ID { get { return Info.ID; } }
    public override ItemInfo_Base.Kinds Kind { get { return Info.Kind; } }
    public override Sprite Icon { get { return Info.Icon; } }
    //-- IUseable --
    public override string Name { get { return Info.Name; } }
    public override string Descript { get { return Info.Descript; } }
    public override string Effect { get { return Info.Effect; } }
    public override int LimitLevel { get { return Info.LimitLevel; } }
    public override int Cost { get { return Info.Cost; } }
    //-- IItem --

    //-- ItemInfo_Consumable --
    public override int StackSize { get { return Info.StackSize; } }

    //-- ItemInfo_Potion --
    public string BuffName { get { return Info.BuffName; } }
    public int Value { get { return Info.Value; } }

    //-- IMoveable --
    public override string GetName()
    {
        string color = string.Empty;
        #region 아이템 이름 색상 설정
        switch (Quality)
        {
            case Qualitys.Normal:
                color = "#d6d6d6";
                break;
            case Qualitys.Advanced:
                color = "#00ff00ff";
                break;
            case Qualitys.Rare:
                color = "#0000ffff";
                break;
            case Qualitys.Epic:
                color = "#800080ff";
                break;
            case Qualitys.Legendary:
                color = "#ffff00ff";
                break;
            case Qualitys.Relic:
                color = "#ff6600ff";
                break;
        }
        #endregion
        return string.Format("<color={0}>{1}</color>", color, Info.Name);
    }

    //-- IUseable --
    public override void Use()
    {
        Player.MyInstance.NewBuff(Info.BuffName);
        Remove();
    }
}
