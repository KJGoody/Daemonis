using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SlotScript : Slot_Base, IStackable
{
    // ���Կ� ��ϵ� ������ ����Ʈ
    // ��ø������ 2�� �̻��� �������� ���� �� �ִ�.
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

    // �� ���� ����
    public bool IsEmpty { get { return MyItems.Count == 0; } }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    // ���Կ� ������ �߰�.
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
        // �ڱ� �ڽ��� �󽽷��� �ƴ϶��
        if (!IsEmpty)
        {
            // Items �� ���� ������ �������� �����ϴ�.
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());

            // �ش� ������ �����۾������� ����ȭ��ŵ�ϴ�.
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
        // �󽽷��� �ƴϰ�
        // �ش� ���Կ� �ִ� ������ �̸���
        // �߰��Ƿ��� �������� �̸��� �����ϴٸ�
        if (!IsEmpty && item.MyName == MyItem.MyName)
        {
            // �������� ��ø������
            // �������� MyStackSize ���� �۴ٸ�
            if (MyItems.Count < (MyItem as Item_Consumable).StackSize)
            {
                // �������� ��ø��ŵ�ϴ�.
                MyItems.Push(item);
                item.MySlot = this;
                return true;
            }
        }
        return false;
    }
}