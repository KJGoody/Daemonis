using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SlotScript : Slot_Base, IStackable
{
    // 슬롯에 등록된 아이템 리스트
    // 중첩개수가 2개 이상인 아이템이 있을 수 있다.
    private ObservableStack<Item_Base> items = new ObservableStack<Item_Base>();
    public ObservableStack<Item_Base> MyItems { get { return items; } }
    public Item_Base MyItem
    {
        get
        {
            if (!IsEmpty)
                return MyItems.Peek();

            return null;
        }
    }

    [SerializeField]
    private TextMeshProUGUI stackSize;
    public TextMeshProUGUI MyStackText { get { return stackSize; } }
    public int MyCount { get { return MyItems.Count; } }

    // 빈 슬롯 여부
    public bool IsEmpty { get { return MyItems.Count == 0; } }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    // 슬롯에 아이템 추가.
    public bool AddItem(Item_Base item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public void RemoveItem()
    {
        // 자기 자신이 빈슬롯이 아니라면
        if (!IsEmpty)
        {
            // Items 의 제일 마지막 아이템을 꺼냅니다.
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());

            // 해당 슬롯의 아이템아이콘을 투명화시킵니다.
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        HandScript.MyInstance.SelectItem(MyItem);
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public bool StackItem(Item_Consumable item)
    {
        // 빈슬롯이 아니고
        // 해당 슬롯에 있는 아이템 이름과
        // 추가되려는 아이템의 이름이 동일하다면
        if (!IsEmpty && item.MyName == MyItem.MyName)
        {
            // 아이템의 중첩개수가
            // 아이템의 MyStackSize 보다 작다면
            if (MyItems.Count < (MyItem as Item_Consumable).StackSize)
            {
                // 아이템을 중첩시킵니다.
                MyItems.Push(item);
                item.MySlot = this;
                return true;
            }
        }
        return false;
    }
}