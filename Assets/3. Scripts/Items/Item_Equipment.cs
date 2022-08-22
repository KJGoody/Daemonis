using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equipment : Item_Base
{
    private ItemInfo_Equipment Info;
    public ItemInfo_Equipment GetInfo() { return Info; }
    public void SetInfo(ItemInfo_Equipment Info) { this.Info = Info; }

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

    //-- ItemInfo_Equipment --
    public ItemInfo_Equipment.Parts Part { get { return Info.Part; } }
    public string GetItemSpriteName(int Index)
    {
        Object[] sprites = Resources.LoadAll("Sprites/" + Info.ItemSprite);
        return sprites[Index].name;
    }
    public Sprite GetItemSprite(int Index)
    {
        Object[] sprites = Resources.LoadAll("Sprites/" + Info.ItemSprite);
        return sprites[Index] as Sprite;
    }
    public void ActiveEquipment(bool isActive) // 장비 착용 & 해제
    {
        Debug.Log(Part);
        // 장비 베이스 스탯 증감
        if (isActive)
            Player.MyInstance.PlusStat(Info.BaseOption, Info.BaseOptionValue);
        else
            Player.MyInstance.PlusStat(Info.BaseOption, -Info.BaseOptionValue);

        // 추가 옵션 증감
        if (addOptionList.Count > 0)
        {
            for (int i = 0; i < addOptionList.Count; i++)
            {
                string optionName = ItemAddOptionScript.Instance.GetNameString(addOptionList[i].Num);
                float optionValue = addOptionList[i].value;
                if (optionName == "ItemLevel")
                {

                }
                else
                {
                    if (isActive) // 추가옵션 스탯적용
                        Player.MyInstance.PlusStat(optionName, optionValue);
                    else
                        Player.MyInstance.PlusStat(optionName, -optionValue);
                }
            }
        }
    }

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
        if (Kind == ItemInfo_Base.Kinds.Equipment)
        {
            Player.MyInstance.EquipItem(this);
            Remove();
        }
    }

    public override void Remove()
    {
        InventoryScript.MyInstance.FindEquipment(this);
        MySlot.RemoveItem();
    }
    public override int GetPriorty()
    {
        return (int)(Mathf.Pow(10, (int)Kind) * ((int)Part + 1) + (int)Quality);
    }

    //-- Item_Equipment --
    public List<ItemAddOption> addOptionList = new List<ItemAddOption>();

    public void SetAddOption()
    {
        for (int i = 0; i < (int)Quality + 1; i++)
        {
            int newQuality = ItemAddOptionScript.Instance.SetRandomQuality(Quality);
            int newAddOption = ItemAddOptionScript.Instance.SetRandomAddOption();
            float newValue = 0;

            addOptionList.Add(new ItemAddOption(newQuality, newAddOption, newValue));
            newValue = ItemAddOptionScript.Instance.SetRandomValue(addOptionList[i]);
            addOptionList[i].value = newValue;
        }
    }

}