using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Potion : Item_Consumable
{
    public ItemInfo_Potion itemInfo;

    public override ItemInfo_Base ItemInfo()
    {
        ItemInfo_Potion itemInfo = this.itemInfo;
        return itemInfo;
    }

    public override void Use()
    {
        Player.MyInstance.NewBuff((ItemInfo() as ItemInfo_Potion).BuffName);
        Remove();
    }
}
