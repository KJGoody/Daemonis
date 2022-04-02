using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    // 슬롯에 등록된 아이템 리스트
    // 중첩개수가 2개 이상인 아이템이 있을 수 있다.
    private Stack<Item> items = new Stack<Item>();


    // 아이템의 아이콘
    [SerializeField]
    private Image icon;

    // 빈 슬롯 여부
    public bool IsEmpty
    {
        get { return items.Count == 0; }
    }

    // 슬롯에 아이템 추가.
    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        return true;
    }
}