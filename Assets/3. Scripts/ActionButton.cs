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
    // ��� ���� ������ ����Ʈ
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
            if (UseableSpell != null)
            {
                // ���� ��Ÿ���� 0�� ��쿡�� ����� �� �ִ�. && �÷��̾��� ���� ������ ��ų �������� ���ƾ� �Ѵ�. && �������� �ƴϿ��� �Ѵ�.
                if (CurrentCollTime == 0 && Player.MyInstance.MyStat.CurrentMana - (UseableSpell as Spell).MySpellMana >= 0 && !Player.MyInstance.IsAttacking)   
                {
                    CoolTime = (UseableSpell as Spell).MySpellCoolTime;
                    StartCoroutine(StartCoolDown());

                    Player.MyInstance.MyStat.CurrentMana -= (UseableSpell as Spell).MySpellMana;

                    UseableSpell.Use();
                }
            }

            // �׼������Կ� ��밡���� �������� ��ϵǾ���
            // �׵�ϵ� �������� ������ 1�� �̻��̶��
            if (UseableItem != null && UseableItem.Count > 0)
            {
                // useables �� ��ϵ� �������� ����մϴ�.
                // Peek() �� �������� �迭���� �������� �ʽ��ϴ�.
                if(CurrentCollTime == 0)
                {   // ���� ��Ÿ�� ���� �� ��ٿ� �̹��� ���� �ڷ�ƾ ����
                    CoolTime = 3f;
                    StartCoroutine(StartCoolDown());
                    Item_Base itemBase = UseableItem.Peek() as Item_Base;
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
                        if(AlreadySetButton.CurrentCollTime == 0)
                        {
                            if(HandScript.MyInstance.MyMoveable is Item_Base)
                            {
                                // �̹� �����Ǿ� �ִ� ��ư�� �ʱ�ȭ
                                AlreadySetButton.UseableItem = new Stack<IUseable>();
                                AlreadySetButton.count = 0;
                                AlreadySetButton.MyIcon.sprite = null;
                                AlreadySetButton.MyIcon.color = new Color(0, 0, 0, 0);
                                UIManager.MyInstance.UpdateStackSize(AlreadySetButton);
                                AlreadySetButton = null;

                                // ���ο� ��ư�� �Ҵ�
                                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                                HandScript.MyInstance.SkillBlindControll();
                            }
                            else
                            {
                                // �̹� �����Ǿ� �ִ� ��ư�� �ʱ�ȭ
                                AlreadySetButton.UseableSpell = null;
                                AlreadySetButton.MyIcon.sprite = null;
                                AlreadySetButton.MyIcon.color = new Color(0, 0, 0, 0);
                                AlreadySetButton = null;

                                // ���ο� ��ư�� �Ҵ�
                                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                                HandScript.MyInstance.SkillBlindControll();
                            }
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
        if (useable is Item_Base)
        {
            // �ش� �����۰� ���� ������ �������� ���� ����Ʈ�� �����ϰ�
            UseableItem = InventoryScript.MyInstance.GetUseables(useable);

            // ���� ����
            count = UseableItem.Count;

            //  �̵���� ���� ����
            //InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            InventoryScript.MyInstance.FromSlot = null;
        }
        else
        {
            // MyUseable.Use()�� ��ư�� Ŭ���Ǿ����� ȣ��ȴ�. 
            // MyUseable�� �������̽��� Spell ���� ��ӹް� �ִ�.
            this.UseableSpell = useable;
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

    public void UpdateItemCount(Item_Base item)
    {
        // �������� IUseable(�������̽�)�� ��ӹ޾�����
        // useables �迭�� �����۰����� 1�� �̻��̸�
        if (item is IUseable && UseableItem.Count > 0)
        {
            // useables �� ��ϵ� �����۰� item �� ���� Ÿ���̶��
            if (UseableItem.Peek().GetName() == item.MyName)
            {

                // �κ��丮���� �ش� �����۰� ���� ��� �������� ã�Ƽ�
                // useables �� ����ϴ�. 
                UseableItem = InventoryScript.MyInstance.GetUseables(item);

                count = UseableItem.Count;

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