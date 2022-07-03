using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo_Consumable : ItemInfo_Base
{
    private int stackSize;  // 중첩 스택
    public int StackSize { get { return stackSize; } }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: 체력 {0} 회복</color>");
    }
}
