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

    public bool AddItem(Item item)
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
