using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToChestPanel : MonoBehaviour
{
    private Stack<Item_Consumable> Items;
    [SerializeField] private Image ItemIcon;
    [SerializeField] private Text ItemName;
    [SerializeField] private Text CountText;
    [SerializeField] private Text ButtonText;
    [HideInInspector] public int Count;

    public void SetMoveToChestPanel(Item_Consumable item)
    {
        ItemIcon.sprite = item.Icon;
        ItemName.text = item.Name;
        foreach(Slot_Inventory slot in GameManager.MyInstance.Slots)
        {
            if (!slot.IsEmpty && slot.Item.Name == item.Name)
                Items.Push(item);
        }
    }

    public void _CountAddSub(int num)
    {
        Count += num;

        if (Count <= 0)
            Count = 1;
        if (Count > Items.Count)
            Count = Items.Count;
    }

    public void _MoveToChest()
    {

    }
}
