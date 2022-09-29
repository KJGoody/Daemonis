using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Slot_Chest : Slot_Stack
{
    public override bool AddItem(Item_Base item)
    {
        item.MySlot = this;
        Items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        return true;
    }

    public override void RemoveItem()
    {
        // 자기 자신이 빈슬롯이 아니라면
        if (!IsEmpty)
        {
            Items.Pop();
            // 해당 슬롯의 아이템아이콘을 투명화시킵니다.
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEmpty)
            ChestPanel.Instance.SelectItemEvent(Item);
    }

    public override bool StackItem(Item_Consumable item)
    {
        // 빈슬롯이 아니고
        // 해당 슬롯에 있는 아이템 이름과
        // 추가되려는 아이템의 이름이 동일하다면
        if (!IsEmpty && item.Name == Item.Name)
        {
            // 아이템의 중첩개수가
            // 아이템의 MyStackSize 보다 작다면
            if (Items.Count < (Item as Item_Consumable).StackSize)
            {
                // 아이템을 중첩시킵니다.
                item.MySlot = this;
                Items.Push(item);
                return true;
            }
        }
        return false;
    }
}
