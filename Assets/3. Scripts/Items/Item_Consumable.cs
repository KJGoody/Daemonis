using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Consumable : Item_Base
{
    public ItemInfo_Consumable itemInfo;
    public override ItemInfo_Base ItemInfo()
    {
        ItemInfo_Consumable itemInfo = this.itemInfo;
        return itemInfo;
    }

    public int StackSize { get { return itemInfo.StackSize; } }

    public override int GetPriorty()
    {
        return (int)(Mathf.Pow(10, (int)Kind) + (int)quality);
    }

    public override void Use()
    {
        if (Kind == ItemInfo_Base.Kinds.Potion)
        {
            HealthPotion healthPotion = itemInfo as HealthPotion;
            healthPotion.Use();
            Remove();
        }
    }

    public override void Remove()
    {
        if (MySlot != null)
        {
            InventoryScript.MyInstance.FindUseSlot(this);
            MySlot.RemoveItem();
        }
    }
}
