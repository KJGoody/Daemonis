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
        // 테스트를 위해 체력을 3씩 감소
        Player.MyInstance.MyStat.CurrentHealth -= 10;

        // 체력물약 아이템 생성
        HealthPotion potion = (HealthPotion)Instantiate(items[0]);

        // 가방에 추가한다.
        AddItem(potion);
    }
    public void OnItemCountChanged(Item item)
    {
        // 이벤트에 등록된 델리게이트에 있다면
        if (itemCountChangedEvent != null)
        {
            // 이벤트에 등록된 모든 델리게이트호출 
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

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();
        // 가방의 모든 슬롯을 검사
        foreach (SlotScript slot in MySlots)
        {
            // 빈슬롯이 아니고
            // 슬롯에 등록된 아이템이 type의 아이템과 같은 종류의 아이템이라면
            if (!slot.IsEmpty && slot.MyItem.MyName == type.GetName())
            {
                
                // 해당 슬롯에 등록된 모든 아이템을
                foreach (Item item in slot.MyItems)
                {
                    // useables 에 담는다.
                    useables.Push(item as IUseable);
                }
            }
        }
        

        return useables;
    }



    public SlotScript FromSlot // 흠
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
                // 슬롯의 색상을 변경합니다.
                fromSlot.MyIcon.color = Color.gray;
            }
        }
    }

}
