using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Consumable : Item_Base
{
    public override ItemInfo_Base ItemInfo() { return null; }
    public int StackSize { get { return (ItemInfo() as ItemInfo_Consumable).StackSize; } }

    public override void Remove()
    {
        if (MySlot != null)
        {
            InventoryScript.MyInstance.FindUseSlot(this);
            MySlot.RemoveItem();
        }
    }

    public override int GetPriorty()
    {
        return (int)(Mathf.Pow(10, (int)Kind) + (int)quality);
    }
}
