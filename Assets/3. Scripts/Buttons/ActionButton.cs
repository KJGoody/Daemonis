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
        // Ŭ�� �̺�Ʈ�� MyButton �� ����Ѵ�.
        MyButton.onClick.AddListener(OnClick);
    }

    // Ŭ�� �߻��ϸ� ����
    public void OnClick()
    {
        if (MyUseable != null)
        {
            MyUseable.Use();
        }
    }

    // Ŭ���� �߻��ߴ��� ����. 
    // IPointerClickHandler �� ��õ� �Լ��̴�.
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.MyInstance.MyMoveable != null)
            {
                // IUseable �� ��ȯ�� �� �ִ��� Ȯ��.
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
        // MyUseable.Use()�� ��ư�� Ŭ���Ǿ����� ȣ��ȴ�. 
        // MyUseable�� �������̽��� Spell ���� ��ӹް� �ִ�.
        this.MyUseable = useable;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        // ActionButton�� �̹����� �����Ѵ�.
        MyIcon.sprite= HandScript.MyInstance.Put().MyIcon;
        MyIcon.color=Color.white;
    }

}
