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

    public void AddItem(Item item)
    {
        // 추가되려는 아이템이 중첩 가능 아이템인지 확인합니다.
        if (item.MyStackSize > 0)
        {
            // 가능하다면 PlaceInStack() 함수를 호출합니다.
            if (PlaceInStack(item))
            {
                return;
            }
        }
        // 중첩이 불가능한 아이템은 빈슬롯에 추가합니다.
        PlaceInEmpty(item);
    }
    private bool PlaceInStack(Item item)
    {
            // 인벤토리 슬롯들을 검사합니다.
            foreach (SlotScript slots in MySlots)
            {
                // 해당 슬롯에 있는 아이템과 중첩시킬 수 있는지 확인합니다.
                // 중첩이 가능하면 아이템을 중첩시키고 반복문을 중단합니다.
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
