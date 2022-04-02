using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    // ���Կ� ��ϵ� ������ ����Ʈ
    // ��ø������ 2�� �̻��� �������� ���� �� �ִ�.
    private ObservableStack<Item> items = new ObservableStack<Item>();


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
            return items.Count;
        }
    }
    // �� ���� ����
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



    private void Awake()
    {
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    // ���Կ� ������ �߰�.
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
        // �ڱ� �ڽ��� �󽽷��� �ƴ϶��
        if (!IsEmpty)
        {
            // Items �� ���� ������ �������� �����ϴ�.
            items.Pop();

            // �ش� ������ �����۾������� ����ȭ��ŵ�ϴ�.
            //UIManager.MyInstance.UpdateStackSize(this);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // ������ ���콺�� ���ȴٸ�
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }
    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public bool StackItem(Item item)
    {
        // �󽽷��� �ƴϰ�
        // �ش� ���Կ� �ִ� ������ �̸���
        // �߰��Ƿ��� �������� �̸��� �����ϴٸ�
        if (!IsEmpty && item.name == MyItem.name)
        {
            // �������� ��ø������
            // �������� MyStackSize ���� �۴ٸ�
            if (items.Count < MyItem.MyStackSize)
            {
                // �������� ��ø��ŵ�ϴ�.
                items.Push(item);
                item.MySlot = this;
                return true;
            }
        }
        return false;
    }

    public void UseItem()
    {
        // �ش� ������ IUseable �������̽��� ��ӹ޾Ҵٸ�
        if (MyItem is IUseable)
        {
            // �ش� �������� ����Ѵ�.
            (MyItem as IUseable).Use();
        }

    }


}