using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
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
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI stackSize;

    
    // 사용 가능 아이템 리스트
    private Stack<IUseable> useables = new Stack<IUseable>();
    private int count;
    public int MyCount
    {
        get
        {
            return count;
        }
    }
    public TextMeshProUGUI MyStackText
    {
        get
        {
            return stackSize;
        }
    }
    public IUseable MyUseable { get; set; }
    public Button MyButton { get; private set; }


    void Start()
    {
        MyButton = GetComponent<Button>();
        // 클릭 이벤트를 MyButton 에 등록한다.
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // 클릭 발생하면 실행
    public void OnClick()
    {
        if (HandScript.MyInstance.MyMoveable == null)
        {
            // 액션퀵슬롯에 등록된 것이 사용할 수 있는거라면
            if (MyUseable != null)
            {
                MyUseable.Use();
            }


            // 액션퀵슬롯에 사용가능한 아이템이 등록되었고
            // 그등록된 아이템의 개수가 1개 이상이라면
            if (useables != null && useables.Count > 0)
            {
                // useables 에 등록된 아이템을 사용합니다.
                // Peek() 은 아이템을 배열에서 제거하지 않습니다.
                useables.Peek().Use();
            }
        }
    }

    // 클릭이 발생했는지 감지. 
    // IPointerClickHandler 에 명시된 함수이다.
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.MyInstance.MyMoveable != null)
            {
                // IUseable 로 변환할 수 있는지 확인.
                if(HandScript.MyInstance.MyMoveable is IUseable)
                {
                    SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                    HandScript.MyInstance.BlindControll();
                }
            }
        }
    }
    public void SetUseable(IUseable useable)
    {
        // 액션 퀵슬롯에 등록되려는 것이 아이템이라면
        if (useable is Item)
        {
            // 해당 아이템과 같은 종류의 아이템을 가진 리스트를 저장하고
            useables = InventoryScript.MyInstance.GetUseables(useable);

            // 개수 저장
            count = useables.Count;

            //  이동모드 상태 해제
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            InventoryScript.MyInstance.FromSlot = null;
        }
        else
        {

            // MyUseable.Use()는 버튼이 클릭되었을때 호출된다. 
            // MyUseable은 인터페이스로 Spell 에서 상속받고 있다.
            this.MyUseable = useable;
        }

        UpdateVisual();
    }


    public void UpdateVisual()
    {
        // ActionButton의 이미지를 변경한다.
        MyIcon.sprite= HandScript.MyInstance.Put().MyIcon;
        MyIcon.color=Color.white;
        if (count > 1)
        {
            // UpdateStackSize 는 아이템 중첩개수를 표시할 때 호출합니다.
            // 매개변수로 Clickable 타입을 받습니다.
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }
    public void UpdateItemCount(Item item)
    {
        // 아이템이 IUseable(인터페이스)을 상속받았으며
        // useables 배열의 아이템개수가 1개 이상이면
        if (item is IUseable && useables.Count > 0)
        {
            // useables 에 등록된 아이템과 item 이 같은 타입이라면
            if (useables.Peek().GetType() == item.GetType())
            {
                // 인벤토리에서 해당 아이템과 같은 모든 아이템을 찾아서
                // useables 에 담습니다. 
                useables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                count = useables.Count;

                // UpdateStackSize 는 아이템 중첩개수를 표시할 때 호출합니다.
                // 매개변수로 Clickable 타입을 받습니다.
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    // 툴팁 켜기
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 액션 버튼에 등록된 것이 스킬이라면
        if (MyUseable != null)
        {
            //UIManager.MyInstance.ShowTooltip(transform.position);
        }

        // 액션 버튼에 등록된 것이 아이템이라면
        else if (useables.Count > 0)
        {
           // UIManager.MyInstance.ShowTooltip(transform.position);
        }
    }

    // 툴팁 끄기
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

}
