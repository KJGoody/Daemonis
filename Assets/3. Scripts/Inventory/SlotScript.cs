using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    // 슬롯에 등록된 아이템 리스트
    // 중첩개수가 2개 이상인 아이템이 있을 수 있다.
    private ObservableStack<ItemBase> items = new ObservableStack<ItemBase>();
    public ObservableStack<ItemBase> MyItems
    {
        get
        {
            return items;
        }
    }
    // 아이템의 아이콘
    [SerializeField]
    private Image icon;
    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }
    [SerializeField]
    private TextMeshProUGUI stackSize;

    public TextMeshProUGUI MyStackText
    {
        get
        {
            return stackSize;
        }
    }
    public int MyCount
    {
        get
        {
            return MyItems.Count;
        }
    }
    // 빈 슬롯 여부
    public bool IsEmpty
    {
        get { return MyItems.Count == 0; }
    }
    public ItemBase MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }
            return null;
        }
    }


    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    // 슬롯에 아이템 추가.
    public bool AddItem(ItemBase item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }
    public void RemoveItem(ItemBase item)
    {
        Debug.Log(MyCount);
        // 자기 자신이 빈슬롯이 아니라면
        if (!IsEmpty)
        {
            Debug.Log("RemoveItem");
            // Items 의 제일 마지막 아이템을 꺼냅니다.
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());

            // 해당 슬롯의 아이템아이콘을 투명화시킵니다.
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
         HandScript.MyInstance.SelectItem(MyItem);
    }
    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public bool StackItem(ItemBase item)
    {
        // 빈슬롯이 아니고
        // 해당 슬롯에 있는 아이템 이름과
        // 추가되려는 아이템의 이름이 동일하다면
        if (!IsEmpty && item.MyName == MyItem.MyName)
        {
            // 아이템의 중첩개수가
            // 아이템의 MyStackSize 보다 작다면
            if (MyItems.Count < MyItem.MyStackSize)
            {
                // 아이템을 중첩시킵니다.
                MyItems.Push(item);
                item.MySlot = this;
                return true;
            }
        }
        return false;
    }
    // 마우스 커서가 Slot 영역 안으로 들어오면 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (!IsEmpty)
        //{
        //    UIManager.MyInstance.ShowTooltip(transform.position, MyItem);
        //}
    }



    // 마우스 커서가 Slot 영역 안에서 밖으로 나가면 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        //UIManager.MyInstance.HideTooltip();
    }

    #region
    public bool IsFull
    {
        get
        {
            // 빈슬롯이거나, 슬롯의 아이템 슬롯 개수가 MyStackSize 보다 작으면
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            else return true;
        }
    }



    private bool PutItemBack()
    {
        // 현재 슬롯과 이동시키려는 슬롯이 같다면
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            // 슬롯의 색상을 원래대로 변경한다.
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }
    //private bool SwapItems(SlotScript from)
    //{
    //    // 슬롯이 비어있다면
    //    if (IsEmpty)
    //    {
    //        return false;
    //    }

    //    // 동일한 아이템이 아니거나
    //    // 이동하려는 아이템 개수 + 현재 아이템 개수 가 아이템의 StackSize 보다 크다면
    //    if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize)
    //    {
    //        ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);

    //        // from 슬롯의 아이템 리스트를 초기화
    //        from.items.Clear();
    //        // from 슬롯의 아이템 리스트에 해당 슬롯의 아이템리스트 전달
    //        from.AddItem();

    //        // 현재 슬롯의 아이템 리스트 초기화
    //        items.Clear();

    //        // 현재 슬롯의 아이템 리스트를 tmpFrom 으로 변경
    //        AddItems(tmpFrom);

    //        return true;
    //    }

    //    return false;
    //}
    public void UseItem()
    {
        // 해당 아이템 IUseable 인터페이스를 상속받았다면
        if (MyItem is IUseable)
        {
            // 해당 아이템을 사용한다.
            (MyItem as IUseable).Use();
        }

    }
    #endregion

}