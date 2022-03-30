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
        // ���� ���¿��� �ۼ��ɵ�.
    }
}