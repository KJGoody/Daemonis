using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(ItemBase item);
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
    private List<SlotScript> slots = new List<SlotScript>();
    // ���濡 ������ �߰��Ѵ�.
    private CanvasGroup canvasGroup;
    public List<SlotScript> MySlots { get { return slots; } }
    private SlotScript fromSlot;
    public ItemBase[] items;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        AddSlots(40);
    }
    
    public void OnItemCountChanged(ItemBase item)
    {
        // �̺�Ʈ�� ��ϵ� ��������Ʈ�� �ִٸ�
        if (itemCountChangedEvent != null)
        {
            // �̺�Ʈ�� ��ϵ� ��� ��������Ʈȣ�� 
            itemCountChangedEvent.Invoke(item);
        }
    }

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slots.Add(slot);
        }
    }

    public void AddItem(ItemBase item)
    {
        // �߰��Ƿ��� �������� ��ø ���� ���������� Ȯ���մϴ�.
        if (item.MyStackSize > 0)
        {
            // �����ϴٸ� PlaceInStack() �Լ��� ȣ���մϴ�.
            if (PlaceInStack(item))
            {
                return;
            }
        }
        // ��ø�� �Ұ����� �������� �󽽷Կ� �߰��մϴ�.
        PlaceInEmpty(item);
    }
    private bool PlaceInStack(ItemBase item)
    {
        // �κ��丮 ���Ե��� �˻��մϴ�.
        foreach (SlotScript slots in MySlots)
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
    public void FindUseSlot(ItemBase item)
    {
        foreach (SlotScript slots in MySlots)
        {
            if (!slots.IsEmpty && slots.MyItem.MyName == item.MyName && slots.MyItem.quality == item.quality)
                item.MySlot = slots;
        }
    }
    public void FindEquipment(ItemBase item)
    {
        foreach (SlotScript slots in MySlots)
        {
            if (!slots.IsEmpty && slots.MyItem.MyName == item.MyName && slots.MyItem.quality == item.quality && slots.MyItem == item)
            {
                item.MySlot = slots;
            }
        }
    }
    private bool PlaceInEmpty(ItemBase item)
    {
        foreach (SlotScript slot in slots)
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
        foreach (SlotScript slot in MySlots)
        {
            // �󽽷��� �ƴϰ�
            // ���Կ� ��ϵ� �������� type�� �����۰� ���� ������ �������̶��
            if (!slot.IsEmpty && slot.MyItem.MyName == type.GetName())
            {

                // �ش� ���Կ� ��ϵ� ��� ��������
                foreach (ItemBase item in slot.MyItems)
                {
                    // useables �� ��´�.
                    useables.Push(item as IUseable);
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
