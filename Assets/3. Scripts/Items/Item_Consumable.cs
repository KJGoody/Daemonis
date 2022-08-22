using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Consumable : Item_Base
{
    public bool IsSetInfo = false;
    public virtual int StackSize { get; } 

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
        return (int)(Mathf.Pow(10, (int)Kind) + (int)Quality);
    }
}
