using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item_Base item);
public class InventoryScript : MonoBehaviour
{
    private static InventoryScript instance;
    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InventoryScript>();
            return instance;
        }
    }
    public event ItemCountChanged itemCountChangedEvent;

    [SerializeField]
    private GameObject slotPrefab;
    // ���� ���� ���� ����Ʈ
    private SlotScript fromSlot;
    public Item_Base[] items;
    
    public void OnItemCountChanged(Item_Base item)
    {
        // �̺�Ʈ�� ��ϵ� ��������Ʈ�� �ִٸ�
        if (itemCountChangedEvent != null)
        {
            // �̺�Ʈ�� ��ϵ� ��� ��������Ʈȣ�� 
            itemCountChangedEvent.Invoke(item);
        }
    }

    public void AddItem(Item_Base item, bool CanStack = false)
    {
        Debug.Log(1);
        // �߰��Ƿ��� �������� ��ø ���� ���������� Ȯ���մϴ�.
        if (CanStack)
        {
            // �����ϴٸ� PlaceInStack() �Լ��� ȣ���մϴ�.
            if (PlaceInStack(item as Item_Consumable))
            {
                return;
            }
        }
        // ��ø�� �Ұ����� �������� �󽽷Կ� �߰��մϴ�.
        PlaceInEmpty(item);
    }

    private bool PlaceInStack(Item_Consumable item)
    {
        // �κ��丮 ���Ե��� �˻��մϴ�.
        foreach (SlotScript slots in GameManager.MyInstance.Slots)
        {
            // �ش� ���Կ� �ִ� �����۰� ��ø��ų �� �ִ��� Ȯ���մϴ�.
            // ��ø�� �����ϸ� �������� ��ø��Ű�� �ݺ����� �ߴ��մϴ�.
            if (slots.StackItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }
        return false;
    }

    public void FindUseSlot(Item_Base item)
    {
        foreach (SlotScript slots in GameManager.MyInstance.Slots)
        {
            if (!slots.IsEmpty && slots.MyItem.MyName == item.MyName && slots.MyItem.quality == item.quality)
                item.MySlot = slots;
        }
    }
    public void FindEquipment(Item_Equipment item)
    {
        foreach (SlotScript slots in GameManager.MyInstance.Slots)
        {
            if (!slots.IsEmpty && slots.MyItem.MyName == item.MyName && slots.MyItem.quality == item.quality && slots.MyItem == item)
            {
                item.MySlot = slots;
            }
        }
    }

    private bool PlaceInEmpty(Item_Base item)
    {
        foreach (SlotScript slot in GameManager.MyInstance.Slots)
        {
            // �� ������ ������
            if (slot.IsEmpty)
            {
                // �ش� ���Կ� �������� �߰��Ѵ�.
                slot.AddItem(item);
                OnItemCountChanged(item);

                return true;
            }
        }
        return false;
    }

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();
        // ������ ��� ������ �˻�
        foreach (SlotScript slot in GameManager.MyInstance.Slots)
        {
            // �󽽷��� �ƴϰ�
            // ���Կ� ��ϵ� �������� type�� �����۰� ���� ������ �������̶��
            if (!slot.IsEmpty && slot.MyItem.MyName == type.GetName())
            {

                // �ش� ���Կ� ��ϵ� ��� ��������
                foreach (Item_Base item in slot.MyItems)
                {
                    // useables �� ��´�.
                    useables.Push(item);
                }
            }
        }


        return useables;
    }



    public SlotScript FromSlot // ��
    {
        get
        {
            return fromSlot;
        }

        set
        {
            fromSlot = value;
            if (value != null)
            {
                // ������ ������ �����մϴ�.
                fromSlot.MyIcon.color = Color.gray;
            }
        }
    }

}
