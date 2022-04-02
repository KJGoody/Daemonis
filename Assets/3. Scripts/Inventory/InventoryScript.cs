using System.Collections.Generic;
using UnityEngine;

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
            // �׽�Ʈ�� ���� ü���� 3�� ����
            Player.MyInstance.MyHealth.MyCurrentValue -= 10;

            // ü�¹��� ������ ����
            HealthPotion potion = (HealthPotion)Instantiate(items[0]);

            // ���濡 �߰��Ѵ�.
            AddItem(potion);
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
                    return true;
                }
            }
        return false;
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

}
