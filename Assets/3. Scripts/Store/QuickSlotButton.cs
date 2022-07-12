using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class QuickSlotButton : ActionButton, IStackable
{
    private Stack<IUseable> Item = new Stack<IUseable>();
    public Stack<IUseable> GetUseableItem
    {
        get
        {
            if (Item.Count == 0)
                return null;
            return Item;
        }
    }
    private int count;
    public int MyCount { get { return count; } }

    [SerializeField]
    private TextMeshProUGUI stackSize;
    public TextMeshProUGUI MyStackText { get { return stackSize; } }

    private QuickSlotButton AlreadySetButton;

    protected override void Start()
    {
        base.Start();
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);

    }

    protected override void OnClick()
    {
        if (HandScript.MyInstance.MyMoveable == null)
        {
            // �׼������Կ� ��밡���� �������� ��ϵǾ���
            // �׵�ϵ� �������� ������ 1�� �̻��̶��
            if (Item != null && Item.Count > 0)
            {
                // useables �� ��ϵ� �������� ����մϴ�.
                // Peek() �� �������� �迭���� �������� �ʽ��ϴ�.
                if (CurrentCollTime == 0)
                {   // ���� ��Ÿ�� ���� �� ��ٿ� �̹��� ���� �ڷ�ƾ ����
                    CoolTime = 3f;
                    StartCoroutine(StartCoolDown());
                    Item_Base itemBase = Item.Peek() as Item_Base;
                    itemBase.Use();
                }
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null)
            {
                // IUseable �� ��ȯ�� �� �ִ��� Ȯ��.
                if (HandScript.MyInstance.MyMoveable is IUseable && CurrentCollTime == 0)
                {
                    // �̹� ActionButton�� ������ �Ǿ��ִ��� Ȯ���Ѵ�.
                    if (IsSetIUseable(HandScript.MyInstance.MyMoveable as IUseable))
                    {
                        if (AlreadySetButton.CurrentCollTime == 0)
                        {
                            if (HandScript.MyInstance.MyMoveable is Item_Base)
                            {
                                // �̹� �����Ǿ� �ִ� ��ư�� �ʱ�ȭ
                                AlreadySetButton.Item = new Stack<IUseable>();
                                AlreadySetButton.count = 0;
                                AlreadySetButton.MyIcon.sprite = null;
                                AlreadySetButton.MyIcon.color = new Color(0, 0, 0, 0);
                                UIManager.MyInstance.UpdateStackSize(AlreadySetButton);
                                AlreadySetButton = null;

                                // ���ο� ��ư�� �Ҵ�
                                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                                HandScript.MyInstance.ResetEquipPotion();
                            }
                        }
                    }
                    // �����Ǿ� ���� �ʴٸ� ���������� ����Ѵ�.
                    else
                    {
                        SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                        if (HandScript.MyInstance.MyMoveable is Item_Base)
                            HandScript.MyInstance.ResetEquipPotion();
                        else
                            HandScript.MyInstance.SkillBlindControll();
                    }
                }
            }
        }
    }

    public override void SetUseable(IUseable useable)
    {
        // �ش� �����۰� ���� ������ �������� ���� ����Ʈ�� �����ϰ�
        Item = InventoryScript.MyInstance.GetUseables(useable);

        // ���� ����
        count = Item.Count;

        //  �̵���� ���� ����
        //InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
        InventoryScript.MyInstance.FromSlot = null;

        base.SetUseable(useable);
    }
    protected override void UpdateVisual(IUseable useable)
    {
        base.UpdateVisual(useable);
        if (count > 1)
        {
            // UpdateStackSize �� ������ ��ø������ ǥ���� �� ȣ���մϴ�.
            // �Ű������� Clickable Ÿ���� �޽��ϴ�.
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(Item_Base item)
    {
        // �������� IUseable(�������̽�)�� ��ӹ޾�����
        // useables �迭�� �����۰����� 1�� �̻��̸�
        if (item is IUseable && Item.Count > 0)
        {
            // useables �� ��ϵ� �����۰� item �� ���� Ÿ���̶��
            if (Item.Peek().GetName() == item.MyName)
            {

                // �κ��丮���� �ش� �����۰� ���� ��� �������� ã�Ƽ�
                // useables �� ����ϴ�. 
                Item = InventoryScript.MyInstance.GetUseables(item);

                count = Item.Count;

                // UpdateStackSize �� ������ ��ø������ ǥ���� �� ȣ���մϴ�.
                // �Ű������� Clickable Ÿ���� �޽��ϴ�.
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    protected override bool IsSetIUseable(IUseable useable)
    {
        foreach (QuickSlotButton quickSlot in GameManager.MyInstance.QuickSlotButtons)
        {
            if (quickSlot.Item.Count > 0)
                if (quickSlot.Item.Peek().GetName().Equals(useable.GetName()))
                {
                    AlreadySetButton = quickSlot;
                    return true;
                }
        }

        return false;
    }
}
