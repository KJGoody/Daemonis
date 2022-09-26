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
        // �ڱ� �ڽ��� �󽽷��� �ƴ϶��
        if (!IsEmpty)
        {
            Items.Pop();
            // �ش� ������ �����۾������� ����ȭ��ŵ�ϴ�.
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
                item.MySlot = this;
                Items.Push(item);
                return true;
            }
        }
        return false;
    }
}