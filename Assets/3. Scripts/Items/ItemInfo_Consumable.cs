using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo_Consumable : ItemInfo_Base
{
    [SerializeField]
    private int stackSize;  // ��ø ����
    public int StackSize { get { return stackSize; } }
}
