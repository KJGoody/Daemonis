using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);
public class InventoryScript : MonoBehaviour
{
    private static InventoryScript instance;
    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }

        set
        {

            instance = value;
        }
    }
    public event ItemCountChanged itemCountChangedEvent;

    [SerializeField]
    private GameObject slotPrefab;
    // ���� ���� ���� ����Ʈ
    private List<SlotScript> slots = new List<SlotScript>();
    // ���濡 ������ �߰��Ѵ�.
    private CanvasGroup canvasGroup;
    public List<SlotScript> MySlots
    {
        get
        {
            return slots;
        }
    }
    private SlotScript fromSlot;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        AddSlots(40);
    }
    public Item[] items;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            asd();
        }
    }
    public void asd()
    {
        // �׽�Ʈ�� ���� ü���� 3�� ����
        Player.MyInstance.MyStat.CurrentHealth -= 10;

        // ü�¹��� ������ ����
        HealthPotion potion = (HealthPotion)Instantiate(items[0]);

        // ���濡 �߰��Ѵ�.
        AddItem(potion);
    }
    public void OnItemCountChanged(Item item)
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

    public void AddItem(Item item)
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
    private bool PlaceInStack(Item item)
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
    public void FindUseSlot(Item item)
    {
        foreach (SlotScript slots in MySlots)
        {
            if(!slots.IsEmpty && slots.MyItem.name == item.name)
               item.MySlot = slots;
        }
       
    }
    private bool PlaceInEmpty(Item item)
    {
        foreach (SlotScript slot in slots)
        {
            // �� ������ ������
            if (slot.IsEmpty)
            {
                // �ش� ���Կ� �������� �߰��Ѵ�.
                slot.AddItem(item);
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
                foreach (Item item in slot.MyItems)
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
