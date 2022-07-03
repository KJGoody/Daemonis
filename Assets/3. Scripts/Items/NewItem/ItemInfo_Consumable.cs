using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo_Consumable : ItemInfo_Base
{
    private int stackSize;  // ��ø ����
    public int StackSize { get { return stackSize; } }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: ü�� {0} ȸ��</color>");
    }
}
