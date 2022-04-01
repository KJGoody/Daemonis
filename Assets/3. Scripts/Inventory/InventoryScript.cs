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
    // 가방 안의 슬롯 리스트
    private List<SlotScript> slots = new List<SlotScript>();
    // 가방에 슬롯을 추가한다.
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
            // 테스트를 위해 체력을 3씩 감소
            Player.MyInstance.MyHealth.MyCurrentValue -= 10;

            // 체력물약 아이템 생성
            HealthPotion potion = (HealthPotion)Instantiate(items[0]);

            // 가방에 추가한다.
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
            // 빈 슬롯이 있으면
            if (slot.IsEmpty)
            {
                // 해당 슬롯에 아이템을 추가한다.
                slot.AddItem(item);
                return true;
            }
        }

        return false;
    }

}
