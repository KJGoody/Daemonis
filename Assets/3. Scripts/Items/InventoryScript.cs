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

    public Item_Base[] items;

    public void OnItemCountChanged(Item_Base item)
    {
        // 이벤트에 등록된 델리게이트에 있다면
        if (itemCountChangedEvent != null)
            // 이벤트에 등록된 모든 델리게이트호출 
            itemCountChangedEvent.Invoke(item);
    }

    public void AddItem(Item_Base item, bool CanStack = false)
    {
        // 추가되려는 아이템이 중첩 가능 아이템인지 확인합니다.
        if (CanStack)
            // 가능하다면 PlaceInStack() 함수를 호출합니다.
            if (PlaceInStack(item as Item_Consumable))
                return;

        // 중첩이 불가능한 아이템은 빈슬롯에 추가합니다.
        PlaceInEmpty(item);
    }

    private bool PlaceInStack(Item_Consumable item)
    {
        // 인벤토리 슬롯들을 검사합니다.
        foreach (SlotScript slots in GameManager.MyInstance.Slots)
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

    public void FindUseSlot(Item_Base item)
    {
        foreach (SlotScript slots in GameManager.MyInstance.Slots)
        {
            if (!slots.IsEmpty && slots.MyItem.Name == item.Name && slots.MyItem.quality == item.quality)
                item.MySlot = slots;
        }
    }

    public void FindEquipment(Item_Equipment item)
    {
        foreach (SlotScript slots in GameManager.MyInstance.Slots)
        {
            if (!slots.IsEmpty && slots.MyItem.Name == item.Name && slots.MyItem.quality == item.quality && slots.MyItem == item)
            {
                item.MySlot = slots;
            }
        }
    }

    private bool PlaceInEmpty(Item_Base item)
    {
        foreach (SlotScript slot in GameManager.MyInstance.Slots)
        {
            // 빈 슬롯이 있으면
            if (slot.IsEmpty)
            {
                // 해당 슬롯에 아이템을 추가한다.
                slot.AddItem(item);
                OnItemCountChanged(item);

                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        SortItem();
    }

    private void SortItem()
    {
        int SlotNum = 28 - GetEmptySlotNum();
        SlotScript[] tempSlot = new SlotScript[SlotNum];
        int j = 0;
        for(int i  = 0; i < 28; i++)
            if (!GameManager.MyInstance.Slots[i].IsEmpty)
                tempSlot[j++] = GameManager.MyInstance.Slots[i];

        int[] IndexArray = new int[SlotNum];
        int[] PriortyArray = new int[SlotNum];

        for(int i = 0; i < SlotNum; i++)
        {
            IndexArray[i] = tempSlot[i].MyItem.GetPriorty();
            PriortyArray[i] = tempSlot[i].MyItem.GetPriorty();
        }

        //Debug.Log(IndexArray);
        //Debug.Log(PriortyArray);
    }

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();
        // 가방의 모든 슬롯을 검사
        foreach (SlotScript slot in GameManager.MyInstance.Slots)
        {
            // 빈슬롯이 아니고
            // 슬롯에 등록된 아이템이 type의 아이템과 같은 종류의 아이템이라면
            if (!slot.IsEmpty && slot.MyItem.Name == type.GetName())
            {

                // 해당 슬롯에 등록된 모든 아이템을
                foreach (Item_Base item in slot.MyItems)
                {
                    // useables 에 담는다.
                    useables.Push(item);
                }
            }
        }

        return useables;
    }

    // 상점에서 구매 할때 빈 슬롯의 알기위한 함수
    public int GetEmptySlotNum()
    {
        int EmptyNum = 0;
        foreach (SlotScript slot in GameManager.MyInstance.Slots)
        {
            if (slot.IsEmpty)
                EmptyNum++;
        }

        return EmptyNum;
    }

    // 상점에서 소모 아이템 구매 할때 구매 가능한 개수를 구하는 함수
    public int CanStackNum(Item_Base Item)
    {
        int CountNum = 0;
        foreach(SlotScript slot in GameManager.MyInstance.Slots)
        {
            if (slot.IsEmpty)
                CountNum += (Item as Item_Consumable).StackSize;
            else
            {
                if(slot.MyItem.Name == Item.Name)
                {
                    if (slot.MyItems.Count < (slot.MyItem as Item_Consumable).StackSize)
                        CountNum += (Item as Item_Consumable).StackSize - slot.MyItems.Count;
                }
            }
        }
        return CountNum;
    }
}
