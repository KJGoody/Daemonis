using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    // 슬롯에 등록된 아이템 리스트
    // 중첩개수가 2개 이상인 아이템이 있을 수 있다.
    private Stack<Item> items = new Stack<Item>();


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

    public int MyCount
    {
        get
        {
            return items.Count;
        }
    }
    // 빈 슬롯 여부
    public bool IsEmpty
    {
        get { return items.Count == 0; }
    }
    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return items.Peek();
            }
            return null;
        }
    }

    // 슬롯에 아이템 추가.
    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }
    public void RemoveItem(Item item)
    {
        // 자기 자신이 빈슬롯이 아니라면
        if (!IsEmpty)
        {
            // Items 의 제일 마지막 아이템을 꺼냅니다.
            items.Pop();

            // 해당 슬롯의 아이템아이콘을 투명화시킵니다.
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 오른쪽 마우스가 눌렸다면
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        // 해당 아이템 IUseable 인터페이스를 상속받았다면
        if (MyItem is IUseable)
        {
            // 해당 아이템을 사용한다.
            (MyItem as IUseable).Use();
        }

    }


}