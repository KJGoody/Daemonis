using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToChestPanel : MonoBehaviour
{
    private Item_Base Item;
    [SerializeField] private Image ItemIcon;
    [SerializeField] private Text ItemName;
    [SerializeField] private Text CountText;
    [SerializeField] private Text ButtonText;
    private int MaxCount;
    private int Count;
    private bool IsTakeOut;

    public void SetMoveToChestPanel(Item_Consumable item, bool isTakeOut = false)
    {
        IsTakeOut = isTakeOut;
        Item = item.Clone();
        ItemIcon.sprite = item.Icon;
        ItemName.text = item.Name;
        MaxCount = 0;
        if (!IsTakeOut)
        {
            foreach(Slot_Inventory slot in GameManager.MyInstance.Slots)
            {
                if (!slot.IsEmpty && slot.Item.Name == item.Name)
                    MaxCount += slot.GetCount;
            }
        }
        else
        {
            foreach (Slot_Chest slot in ChestPanel.Instance.Slots)
            {
                if (!slot.IsEmpty && slot.Item.Name == item.Name)
                    MaxCount += slot.GetCount;
            }
        }
        Count = 1;
        CountText.text = Count.ToString();
        gameObject.SetActive(true);
    }

    public void _CountAddSub(int num)
    {
        Count += num;

        if (Count <= 0)
            Count = 1;
        if (Count > MaxCount)
            Count = MaxCount;

        CountText.text = Count.ToString();
    }

    public void _MoveToChest()
    {
        if (!IsTakeOut)
        {
            if(Count <= ChestPanel.Instance.CanStackNum(Item))
            {
                for(int i = 0; i < Count; i++)
                {
                    InventoryScript.MyInstance.GetItem(Item).RemoveItem();
                    ChestPanel.Instance.AddItem((Item as Item_Consumable).Clone(), true);
                }
                HandScript.MyInstance.Close_SI_Panel();
            }
        }
        else
        {
            if(Count <= InventoryScript.MyInstance.CanStackNum(Item))
            {
                for (int i = 0; i < Count; i++)
                {
                    ChestPanel.Instance.GetItem(Item).RemoveItem();
                    InventoryScript.MyInstance.AddItem((Item as Item_Consumable).Clone(), true);
                }
                ChestPanel.Instance._Close();
            }
        }

        Item = null;
        gameObject.SetActive(false);
    }
}
