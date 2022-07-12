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
            // 액션퀵슬롯에 사용가능한 아이템이 등록되었고
            // 그등록된 아이템의 개수가 1개 이상이라면
            if (Item != null && Item.Count > 0)
            {
                // useables 에 등록된 아이템을 사용합니다.
                // Peek() 은 아이템을 배열에서 제거하지 않습니다.
                if (CurrentCollTime == 0)
                {   // 물약 쿨타임 설정 후 쿨다운 이미지 생성 코루틴 시작
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
                // IUseable 로 변환할 수 있는지 확인.
                if (HandScript.MyInstance.MyMoveable is IUseable && CurrentCollTime == 0)
                {
                    // 이미 ActionButton에 설정이 되어있는지 확인한다.
                    if (IsSetIUseable(HandScript.MyInstance.MyMoveable as IUseable))
                    {
                        if (AlreadySetButton.CurrentCollTime == 0)
                        {
                            if (HandScript.MyInstance.MyMoveable is Item_Base)
                            {
                                // 이미 설정되어 있는 버튼을 초기화
                                AlreadySetButton.Item = new Stack<IUseable>();
                                AlreadySetButton.count = 0;
                                AlreadySetButton.MyIcon.sprite = null;
                                AlreadySetButton.MyIcon.color = new Color(0, 0, 0, 0);
                                UIManager.MyInstance.UpdateStackSize(AlreadySetButton);
                                AlreadySetButton = null;

                                // 새로운 버튼에 할당
                                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                                HandScript.MyInstance.ResetEquipPotion();
                            }
                        }
                    }
                    // 설정되어 있지 않다면 정상적으로 등록한다.
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
        // 해당 아이템과 같은 종류의 아이템을 가진 리스트를 저장하고
        Item = InventoryScript.MyInstance.GetUseables(useable);

        // 개수 저장
        count = Item.Count;

        //  이동모드 상태 해제
        //InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
        InventoryScript.MyInstance.FromSlot = null;

        base.SetUseable(useable);
    }
    protected override void UpdateVisual(IUseable useable)
    {
        base.UpdateVisual(useable);
        if (count > 1)
        {
            // UpdateStackSize 는 아이템 중첩개수를 표시할 때 호출합니다.
            // 매개변수로 Clickable 타입을 받습니다.
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(Item_Base item)
    {
        // 아이템이 IUseable(인터페이스)을 상속받았으며
        // useables 배열의 아이템개수가 1개 이상이면
        if (item is IUseable && Item.Count > 0)
        {
            // useables 에 등록된 아이템과 item 이 같은 타입이라면
            if (Item.Peek().GetName() == item.MyName)
            {

                // 인벤토리에서 해당 아이템과 같은 모든 아이템을 찾아서
                // useables 에 담습니다. 
                Item = InventoryScript.MyInstance.GetUseables(item);

                count = Item.Count;

                // UpdateStackSize 는 아이템 중첩개수를 표시할 때 호출합니다.
                // 매개변수로 Clickable 타입을 받습니다.
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
