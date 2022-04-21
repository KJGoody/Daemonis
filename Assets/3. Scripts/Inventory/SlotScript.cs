using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    // ���Կ� ��ϵ� ������ ����Ʈ
    // ��ø������ 2�� �̻��� �������� ���� �� �ִ�.
    private ObservableStack<ItemBase> items = new ObservableStack<ItemBase>();
    public ObservableStack<ItemBase> MyItems
    {
        get
        {
            return items;
        }
    }
    // �������� ������
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
    // �� ���� ����
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

    // ���Կ� ������ �߰�.
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
        // �ڱ� �ڽ��� �󽽷��� �ƴ϶��
        if (!IsEmpty)
        {
            Debug.Log("RemoveItem");
            // Items �� ���� ������ �������� �����ϴ�.
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());

            // �ش� ������ �����۾������� ����ȭ��ŵ�ϴ�.
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
        // �󽽷��� �ƴϰ�
        // �ش� ���Կ� �ִ� ������ �̸���
        // �߰��Ƿ��� �������� �̸��� �����ϴٸ�
        if (!IsEmpty && item.MyName == MyItem.MyName)
        {
            // �������� ��ø������
            // �������� MyStackSize ���� �۴ٸ�
            if (MyItems.Count < MyItem.MyStackSize)
            {
                // �������� ��ø��ŵ�ϴ�.
                MyItems.Push(item);
                item.MySlot = this;
                return true;
            }
        }
        return false;
    }
    // ���콺 Ŀ���� Slot ���� ������ ������ ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (!IsEmpty)
        //{
        //    UIManager.MyInstance.ShowTooltip(transform.position, MyItem);
        //}
    }



    // ���콺 Ŀ���� Slot ���� �ȿ��� ������ ������ ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        //UIManager.MyInstance.HideTooltip();
    }

    #region
    public bool IsFull
    {
        get
        {
            // �󽽷��̰ų�, ������ ������ ���� ������ MyStackSize ���� ������
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            else return true;
        }
    }



    private bool PutItemBack()
    {
        // ���� ���԰� �̵���Ű���� ������ ���ٸ�
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            // ������ ������ ������� �����Ѵ�.
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }
    //private bool SwapItems(SlotScript from)
    //{
    //    // ������ ����ִٸ�
    //    if (IsEmpty)
    //    {
    //        return false;
    //    }

    //    // ������ �������� �ƴϰų�
    //    // �̵��Ϸ��� ������ ���� + ���� ������ ���� �� �������� StackSize ���� ũ�ٸ�
    //    if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize)
    //    {
    //        ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);

    //        // from ������ ������ ����Ʈ�� �ʱ�ȭ
    //        from.items.Clear();
    //        // from ������ ������ ����Ʈ�� �ش� ������ �����۸���Ʈ ����
    //        from.AddItem();

    //        // ���� ������ ������ ����Ʈ �ʱ�ȭ
    //        items.Clear();

    //        // ���� ������ ������ ����Ʈ�� tmpFrom ���� ����
    //        AddItems(tmpFrom);

    //        return true;
    //    }

    //    return false;
    //}
    public void UseItem()
    {
        // �ش� ������ IUseable �������̽��� ��ӹ޾Ҵٸ�
        if (MyItem is IUseable)
        {
            // �ش� �������� ����Ѵ�.
            (MyItem as IUseable).Use();
        }

    }
    #endregion

}