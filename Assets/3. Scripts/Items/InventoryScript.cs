using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    #region �̱���
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
    #endregion

    public delegate void ItemCountChanged(Item_Base item);
    public event ItemCountChanged itemCountChangedEvent;

    public void OnItemCountChanged(Item_Base item)
    {
        // �̺�Ʈ�� ��ϵ� ��������Ʈ�� �ִٸ�
        if (itemCountChangedEvent != null)
            // �̺�Ʈ�� ��ϵ� ��� ��������Ʈȣ�� 
            itemCountChangedEvent.Invoke(item);
    }

    public void AddItem(Item_Base item, bool CanStack = false)
    {
        // �߰��Ƿ��� �������� ��ø ���� ���������� Ȯ���մϴ�.
        if (CanStack)
            // �����ϴٸ� PlaceInStack() �Լ��� ȣ���մϴ�.
            if (PlaceInStack(item as Item_Consumable))
                return;

        // ��ø�� �Ұ����� �������� �󽽷Կ� �߰��մϴ�.
        PlaceInEmpty(item);
    }

    private bool PlaceInStack(Item_Consumable item)
    {
        // �κ��丮 ���Ե��� �˻��մϴ�.
        foreach (Slot_Inventory slots in GameManager.MyInstance.Slots)
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

    private void PlaceInEmpty(Item_Base item)
    {
        foreach (Slot_Inventory slot in GameManager.MyInstance.Slots)
            // �� ������ ������
            if (slot.IsEmpty)
            {
                // �ش� ���Կ� �������� �߰��Ѵ�.
                slot.AddItem(item);
                OnItemCountChanged(item);
                return;
            }
    }

    public void FindUseSlot(Item_Base item)
    {
        foreach (Slot_Inventory slots in GameManager.MyInstance.Slots)
        {
            if (!slots.IsEmpty && slots.Item.Name == item.Name && slots.Item.Quality == item.Quality)
                item.MySlot = slots;
        }
    }

    public void FindEquipment(Item_Equipment item)
    {
        foreach (Slot_Inventory slots in GameManager.MyInstance.Slots)
        {
            if (!slots.IsEmpty && slots.Item.Name == item.Name && slots.Item.Quality == item.Quality && slots.Item == item)
            {
                item.MySlot = slots;
            }
        }
    }

    private void Start()
    {
        SortItem();
    }

    // ����
    private void SortItem()
    {
        int SlotNum = 28 - GetEmptySlotNum();
        Slot_Inventory[] tempSlot = new Slot_Inventory[SlotNum];
        int j = 0;
        for(int i  = 0; i < 28; i++)
            if (!GameManager.MyInstance.Slots[i].IsEmpty)
                tempSlot[j++] = GameManager.MyInstance.Slots[i];

        int[] IndexArray = new int[SlotNum];
        int[] PriortyArray = new int[SlotNum];

        for(int i = 0; i < SlotNum; i++)
        {
            IndexArray[i] = tempSlot[i].Item.GetPriorty();
            PriortyArray[i] = tempSlot[i].Item.GetPriorty();
        }
    }

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();
        // ������ ��� ������ �˻�
        foreach (Slot_Inventory slot in GameManager.MyInstance.Slots)
        {
            // �󽽷��� �ƴϰ�
            // ���Կ� ��ϵ� �������� type�� �����۰� ���� ������ �������̶��
            if (!slot.IsEmpty && slot.Item.Name == type.Name)
            {
                // �ش� ���Կ� ��ϵ� ��� ��������
                foreach (Item_Base item in slot.GetItems)
                {
                    // useables �� ��´�.
                    useables.Push(item);
                }
            }
        }

        return useables;
    }

    // �������� ���� �Ҷ� �� ������ �˱����� �Լ�
    public int GetEmptySlotNum()
    {
        int EmptyNum = 0;
        foreach (Slot_Inventory slot in GameManager.MyInstance.Slots)
        {
            if (slot.IsEmpty)
                EmptyNum++;
        }

        return EmptyNum;
    }

    // �������� �Ҹ� ������ ���� �Ҷ� ���� ������ ������ ���ϴ� �Լ�
    public int CanStackNum(Item_Base Item)
    {
        int CountNum = 0;
        foreach(Slot_Inventory slot in GameManager.MyInstance.Slots)
        {
            if (slot.IsEmpty)
                CountNum += (Item as Item_Consumable).StackSize;
            else
            {
                if(slot.Item.Name == Item.Name)
                {
                    if (slot.GetItems.Count < (slot.Item as Item_Consumable).StackSize)
                        CountNum += (Item as Item_Consumable).StackSize - slot.GetItems.Count;
                }
            }
        }
        return CountNum;
    }
}
