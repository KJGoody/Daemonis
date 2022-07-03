using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable
{
    public Image icon;
    public Image MyIcon
    {
        get { return icon; }
        set { icon = value; }
    }

    [SerializeField]
    private TextMeshProUGUI stackSize;
    public TextMeshProUGUI MyStackText { get { return stackSize; } }

    private IUseable UseableSpell;
    // 사용 가능 아이템 리스트
    private Stack<IUseable> UseableItem = new Stack<IUseable>();
    private int count;
    public int MyCount { get { return count; } }
    public Button MyButton { get; private set; }

    [SerializeField]
    private Image CoolTimeFillImage;
    private float CoolTime;
    private float CurrentCollTime = 0f;

    private ActionButton AlreadySetButton;

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
            if (UseableSpell != null)
            {
                // 현재 쿨타임이 0일 경우에만 사용할 수 있다. && 플레이어의 현재 마나가 스킬 마나보다 많아야 한다. && 공격중이 아니여야 한다.
                if (CurrentCollTime == 0 && Player.MyInstance.MyStat.CurrentMana - (UseableSpell as Spell).MySpellMana >= 0 && !Player.MyInstance.IsAttacking)   
                {
                    CoolTime = (UseableSpell as Spell).MySpellCoolTime;
                    StartCoroutine(StartCoolDown());

                    Player.MyInstance.MyStat.CurrentMana -= (UseableSpell as Spell).MySpellMana;

                    UseableSpell.Use();
                }
            }

            // 액션퀵슬롯에 사용가능한 아이템이 등록되었고
            // 그등록된 아이템의 개수가 1개 이상이라면
            if (UseableItem != null && UseableItem.Count > 0)
            {
                // useables 에 등록된 아이템을 사용합니다.
                // Peek() 은 아이템을 배열에서 제거하지 않습니다.
                if(CurrentCollTime == 0)
                {   // 물약 쿨타임 설정 후 쿨다운 이미지 생성 코루틴 시작
                    CoolTime = 3f;
                    StartCoroutine(StartCoolDown());
                    Item_Base itemBase = UseableItem.Peek() as Item_Base;
                    itemBase.Use();
                }
            }
        }
    }

    // 클릭이 발생했는지 감지. 
    // IPointerClickHandler 에 명시된 함수이다.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null)
            {
                // IUseable 로 변환할 수 있는지 확인.
                if (HandScript.MyInstance.MyMoveable is IUseable && CurrentCollTime == 0)
                {
                    // 이미 ActionButton에 설정이 되어있는지 확인한다.
                    if(IsSetIUseable(HandScript.MyInstance.MyMoveable as IUseable))
                    {
                        if(AlreadySetButton.CurrentCollTime == 0)
                        {
                            if(HandScript.MyInstance.MyMoveable is Item_Base)
                            {
                                // 이미 설정되어 있는 버튼을 초기화
                                AlreadySetButton.UseableItem = new Stack<IUseable>();
                                AlreadySetButton.count = 0;
                                AlreadySetButton.MyIcon.sprite = null;
                                AlreadySetButton.MyIcon.color = new Color(0, 0, 0, 0);
                                UIManager.MyInstance.UpdateStackSize(AlreadySetButton);
                                AlreadySetButton = null;

                                // 새로운 버튼에 할당
                                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                                HandScript.MyInstance.SkillBlindControll();
                            }
                            else
                            {
                                // 이미 설정되어 있는 버튼을 초기화
                                AlreadySetButton.UseableSpell = null;
                                AlreadySetButton.MyIcon.sprite = null;
                                AlreadySetButton.MyIcon.color = new Color(0, 0, 0, 0);
                                AlreadySetButton = null;

                                // 새로운 버튼에 할당
                                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                                HandScript.MyInstance.SkillBlindControll();
                            }
                        }
                    }
                    // 설정되어 있지 않다면 정상적으로 등록한다.
                    else
                    {
                        SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                        HandScript.MyInstance.SkillBlindControll();
                    }
                }
            }
        }
    }

    public void SetUseable(IUseable useable)
    {
        // 액션 퀵슬롯에 등록되려는 것이 아이템이라면
        if (useable is Item_Base)
        {
            // 해당 아이템과 같은 종류의 아이템을 가진 리스트를 저장하고
            UseableItem = InventoryScript.MyInstance.GetUseables(useable);

            // 개수 저장
            count = UseableItem.Count;

            //  이동모드 상태 해제
            //InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            InventoryScript.MyInstance.FromSlot = null;
        }
        else
        {
            // MyUseable.Use()는 버튼이 클릭되었을때 호출된다. 
            // MyUseable은 인터페이스로 Spell 에서 상속받고 있다.
            this.UseableSpell = useable;
        }
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        // ActionButton의 이미지를 변경한다.
        MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
        MyIcon.color = Color.white;
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
        if (item is IUseable && UseableItem.Count > 0)
        {
            // useables 에 등록된 아이템과 item 이 같은 타입이라면
            if (UseableItem.Peek().GetName() == item.MyName)
            {

                // 인벤토리에서 해당 아이템과 같은 모든 아이템을 찾아서
                // useables 에 담습니다. 
                UseableItem = InventoryScript.MyInstance.GetUseables(item);

                count = UseableItem.Count;

                // UpdateStackSize 는 아이템 중첩개수를 표시할 때 호출합니다.
                // 매개변수로 Clickable 타입을 받습니다.
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    private IEnumerator StartCoolDown()
    {   // 재사용 대기시간 이미지를 보이도록 하는 코루틴
        CoolTimeFillImage.gameObject.SetActive(true);

        CurrentCollTime = CoolTime;
        while (CurrentCollTime > 0)
        {
            CurrentCollTime -= 0.1f;
            CoolTimeFillImage.fillAmount = CurrentCollTime / CoolTime;
            yield return new WaitForSeconds(0.1f);
        }
        CoolTimeFillImage.fillAmount = 0;
        CurrentCollTime = 0;

        CoolTimeFillImage.gameObject.SetActive(false);
    }

    private bool IsSetIUseable(IUseable useable)
    {   // IUseable이 이미 설정이 되어있는지 확인하는 함수
        foreach (ActionButton actionButton in GameManager.MyInstance.ActionButtons)
        {
            if (actionButton.UseableSpell != null)
                if (actionButton.UseableSpell.GetName().Equals(useable.GetName()))
                {
                    AlreadySetButton = actionButton;
                    return true;
                }

            if (actionButton.UseableItem.Count > 0)
                if (actionButton.UseableItem.Peek().GetName().Equals(useable.GetName()))
                {
                    AlreadySetButton = actionButton;
                    return true;
                }
        }

        return false;
    }
}