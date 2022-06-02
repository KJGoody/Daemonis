using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image icon;
    public Image MyIcon
    {
        get { return icon; }
        set { icon = value; }
    }

    [SerializeField]
    private TextMeshProUGUI stackSize;
    // ��� ���� ������ ����Ʈ

    private Stack<IUseable> useables = new Stack<IUseable>();
    private int count;
    public int MyCount { get { return count; } }
    public TextMeshProUGUI MyStackText { get { return stackSize; } }
    public IUseable MyUseable { get; set; }
    public Button MyButton { get; private set; }

    [SerializeField]
    private Image CoolTimeFillImage;
    private float CoolTime;
    private float CurrentCollTime = 0f;

    void Start()
    {
        MyButton = GetComponent<Button>();
        // Ŭ�� �̺�Ʈ�� MyButton �� ����Ѵ�.
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // Ŭ�� �߻��ϸ� ����
    public void OnClick()
    {
        if (HandScript.MyInstance.MyMoveable == null)
        {
            // �׼������Կ� ��ϵ� ���� ����� �� �ִ°Ŷ��
            if (MyUseable != null)
            {
                if (CurrentCollTime == 0)   // ���� ��Ÿ���� 0�� ��쿡�� ����� �� �ִ�.
                {
                    CoolTime = (MyUseable as Spell).MySpellCoolTime;
                    StartCoroutine(StartCoolDown());
                    MyUseable.Use();
                }
            }

            // �׼������Կ� ��밡���� �������� ��ϵǾ���
            // �׵�ϵ� �������� ������ 1�� �̻��̶��
            if (useables != null && useables.Count > 0)
            {
                // useables �� ��ϵ� �������� ����մϴ�.
                // Peek() �� �������� �迭���� �������� �ʽ��ϴ�.
                Debug.Log(useables.Count);
                useables.Peek().Use();
            }
        }
    }

    // Ŭ���� �߻��ߴ��� ����. 
    // IPointerClickHandler �� ��õ� �Լ��̴�.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null)
            {
                // IUseable �� ��ȯ�� �� �ִ��� Ȯ��.
                if (HandScript.MyInstance.MyMoveable is IUseable && CurrentCollTime == 0)
                {
                    SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                    HandScript.MyInstance.SkillBlindControll();
                }
            }
        }
    }

    public void SetUseable(IUseable useable)
    {
        // �׼� �����Կ� ��ϵǷ��� ���� �������̶��
        if (useable is ItemBase)
        {
            // �ش� �����۰� ���� ������ �������� ���� ����Ʈ�� �����ϰ�
            useables = InventoryScript.MyInstance.GetUseables(useable);

            // ���� ����
            count = useables.Count;

            //  �̵���� ���� ����
            //InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            InventoryScript.MyInstance.FromSlot = null;

        }
        else
        {
            // MyUseable.Use()�� ��ư�� Ŭ���Ǿ����� ȣ��ȴ�. 
            // MyUseable�� �������̽��� Spell ���� ��ӹް� �ִ�.
            this.MyUseable = useable;
        }
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        // ActionButton�� �̹����� �����Ѵ�.
        MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
        MyIcon.color = Color.white;
        if (count > 1)
        {
            // UpdateStackSize �� ������ ��ø������ ǥ���� �� ȣ���մϴ�.
            // �Ű������� Clickable Ÿ���� �޽��ϴ�.
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(ItemBase item)
    {
        // �������� IUseable(�������̽�)�� ��ӹ޾�����
        // useables �迭�� �����۰����� 1�� �̻��̸�
        if (item is IUseable && useables.Count > 0)
        {
            // useables �� ��ϵ� �����۰� item �� ���� Ÿ���̶��
            if (useables.Peek().GetName() == item.MyName)
            {

                // �κ��丮���� �ش� �����۰� ���� ��� �������� ã�Ƽ�
                // useables �� ����ϴ�. 
                useables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                count = useables.Count;

                // UpdateStackSize �� ������ ��ø������ ǥ���� �� ȣ���մϴ�.
                // �Ű������� Clickable Ÿ���� �޽��ϴ�.
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    // ���� �ѱ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �׼� ��ư�� ��ϵ� ���� ��ų�̶��
        if (MyUseable != null)
        {
            //UIManager.MyInstance.ShowTooltip(transform.position);
        }

        // �׼� ��ư�� ��ϵ� ���� �������̶��
        else if (useables.Count > 0)
        {
            // UIManager.MyInstance.ShowTooltip(transform.position);
        }
    }

    // ���� ����
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    private IEnumerator StartCoolDown()
    {
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
}
