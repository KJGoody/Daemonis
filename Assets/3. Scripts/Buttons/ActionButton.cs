using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerClickHandler
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


    public IUseable MyUseable { get; set; }
    public Button MyButton { get; private set; }


    void Start()
    {
        MyButton = GetComponent<Button>();
        // 클릭 이벤트를 MyButton 에 등록한다.
        MyButton.onClick.AddListener(OnClick);
    }

    // 클릭 발생하면 실행
    public void OnClick()
    {
        if (MyUseable != null)
        {
            MyUseable.Use();
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
        // MyUseable.Use()는 버튼이 클릭되었을때 호출된다. 
        // MyUseable은 인터페이스로 Spell 에서 상속받고 있다.
        this.MyUseable = useable;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        // ActionButton의 이미지를 변경한다.
        MyIcon.sprite= HandScript.MyInstance.Put().MyIcon;
        MyIcon.color=Color.white;
    }

}
