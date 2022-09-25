using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Slot_Chest : Slot_Stack
{
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
            // �ش� ������ �����۾������� ����ȭ��ŵ�ϴ�.
            UIManager.MyInstance.UpdateStackSize(this);
            return true;
        }
        return false;
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
                Items.Push(item);
                item.MySlot = this;
                return true;
            }
        }
        return false;
    }
}
