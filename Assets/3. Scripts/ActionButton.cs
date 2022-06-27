using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable
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

    public Stack<IUseable> useables = new Stack<IUseable>();
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
                // ���� ��Ÿ���� 0�� ��쿡�� ����� �� �ִ�. && �÷��̾��� ���� ������ ��ų �������� ���ƾ� �Ѵ�. && �������� �ƴϿ��� �Ѵ�.
                if (CurrentCollTime == 0 && Player.MyInstance.MyStat.CurrentMana - (MyUseable as Spell).MySpellMana >= 0 && !Player.MyInstance.IsAttacking)   
                {
                    CoolTime = (MyUseable as Spell).MySpellCoolTime;
                    StartCoroutine(StartCoolDown());

                    Player.MyInstance.MyStat.CurrentMana -= (MyUseable as Spell).MySpellMana;

                    MyUseable.Use();
                }
            }

            // �׼������Կ� ��밡���� �������� ��ϵǾ���
            // �׵�ϵ� �������� ������ 1�� �̻��̶��
            if (useables != null && useables.Count > 0)
            {
                // useables �� ��ϵ� �������� ����մϴ�.
                // Peek() �� �������� �迭���� �������� �ʽ��ϴ�.
                if(CurrentCollTime == 0)
                {   // ���� ��Ÿ�� ���� �� ��ٿ� �̹��� ���� �ڷ�ƾ ����
                    CoolTime = 3f;
                    StartCoroutine(StartCoolDown());
                    ItemBase itemBase = useables.Peek() as ItemBase;
                    itemBase.Use();
                }
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
                    // �̹� ActionButton�� ������ �Ǿ��ִ��� Ȯ���Ѵ�.
                    if(IsSetIUseable(HandScript.MyInstance.MyMoveable as IUseable))
                    {
                        foreach (ActionButton actionButton in GameManager.MyInstance.DATA.ActionButtons)
                            if(actionButton.MyUseable != null)
                                // �̹� �����Ǿ� �ִ� ActionButton�� ã�´�.
                                if (actionButton.MyUseable.GetName().Equals((HandScript.MyInstance.MyMoveable as IUseable).GetName()))
                                {
                                    // �����Ǿ� �ִ� ActionButton�� �ʱ�ȭ�Ѵ�.
                                    actionButton.MyUseable = null;
                                    actionButton.MyIcon.sprite = null;
                                    actionButton.MyIcon.color = new Color(0, 0, 0, 0); 
                                    // �������
                                    for (int i = 0; i < 9; i++)
                                        if (GameManager.MyInstance.DATA.ActionButtons[i] == actionButton)
                                        {
                                            GameManager.MyInstance.DATA.ActionButtonIUseable[i] = null;
                                            break;
                                        }

                                    SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                                    HandScript.MyInstance.SkillBlindControll();
                                    break;
                                }
                    }
                    // �����Ǿ� ���� �ʴٸ� ���������� ����Ѵ�.
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

        // ����
        for(int i = 0; i < 9; i++)
            if(GameManager.MyInstance.DATA.ActionButtons[i] == this)
            {
                GameManager.MyInstance.DATA.ActionButtonIUseable[i] = useable;
                break;
            }
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

    private IEnumerator StartCoolDown()
    {   // ���� ���ð� �̹����� ���̵��� �ϴ� �ڷ�ƾ
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
    {   // IUseable�� �̹� ������ �Ǿ��ִ��� Ȯ���ϴ� �Լ�
        foreach (ActionButton actionButton in GameManager.MyInstance.DATA.ActionButtons)
        {
            if (actionButton.MyUseable != null)
                if (actionButton.MyUseable.GetName().Equals(useable.GetName()))
                    return true;
        }

        return false;
    }
}
