using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equipment : Item_Base
{
    public ItemInfo_Equipment itemInfo;
    public override ItemInfo_Base ItemInfo()
    {
        ItemInfo_Equipment itemInfo = this.itemInfo;
        return itemInfo;
    }

    public List<ItemAddOption> addOptionList = new List<ItemAddOption>();
    public void SetAddOption() // 추가옵션 설정
    {
        for (int i = 0; i < (int)(quality) + 1; i++)
        {
            int newQuality = ItemAddOptionScript.Instance.SetRandomQuality(quality);
            int newAddOption = ItemAddOptionScript.Instance.SetRandomAddOption();
            float newValue = 0;

            addOptionList.Add(new ItemAddOption(newQuality, newAddOption, newValue));
            newValue = ItemAddOptionScript.Instance.SetRandomValue(addOptionList[i]);
            addOptionList[i].value = newValue;
        }
    }

    public ItemInfo_Equipment.Part GetPart { get { return (ItemInfo() as ItemInfo_Equipment).GetPart; } }

    public Sprite[] ItemSprite { get { return (ItemInfo() as ItemInfo_Equipment).GetItemSprite; } }

    public float GetWeaponxDamage()
    {
        ItemInfo_Equipment equipmentItem = itemInfo;
        return (ItemInfo() as ItemInfo_Equipment).GetWeaponxDamage();
    }

    public void ActiveEquipment(bool isActive) // 장비 착용 & 해제
    {
        (ItemInfo() as ItemInfo_Equipment).ActiveEquipmentStat(isActive); // 장비 베이스 스탯 증감
        if (addOptionList.Count > 0) // 추가 옵션 증감
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

    public override void Use()
    {
        if (this.GetKind == ItemInfo_Base.Kinds.Equipment)
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
}
