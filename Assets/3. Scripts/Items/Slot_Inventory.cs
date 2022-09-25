using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Slot_Inventory : Slot_Stack
{
    // ���Կ� ������ �߰�.
    public override bool AddItem(Item_Base item)
    {
        Items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public override bool RemoveItem()
    {
        // �ڱ� �ڽ��� �󽽷��� �ƴ϶��
        if (!IsEmpty)
        {
            // Items �� ���� ������ �������� �����ϴ�.
            InventoryScript.MyInstance.OnItemCountChanged(Items.Pop());
            // �ش� ������ �����۾������� ����ȭ��ŵ�ϴ�.
            UIManager.MyInstance.UpdateStackSize(this);
            return true;
        }
        return false;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEmpty)
            HandScript.MyInstance.SelectItem(Item);
    }

    public override bool StackItem(Item_Consumable item)
    {
        // �󽽷��� �ƴϰ�
        // �ش� ���Կ� �ִ� ������ �̸���
        // �߰��Ƿ��� �������� �̸��� �����ϴٸ�
        if (!IsEmpty && item.Name == Item.Name)
        {
            // �������� ��ø������
            // �������� MyStackSize ���� �۴ٸ�
            if (Items.Count < (Item as Item_Consumable).StackSize)
            {
                // �������� ��ø��ŵ�ϴ�.
                Items.Push(item);
                item.MySlot = this;
                return true;
            }
        }
        return false;
    }
}