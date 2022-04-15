using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    // �̱���
    private static HandScript instance;
    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }
    #region ��ų ���ð��� ����
    [SerializeField]
    private Text selectName;
    [SerializeField]
    private Image selectImage;
    [SerializeField]
    private Text selectDescription;
    [SerializeField]
    private GameObject blindPanel;
    private bool skillEquipping = false;
    #endregion
    #region ������ ���ð��� ���� SI = Select Item
    private Item myItem;    // ������ ����
    [SerializeField]
    private Image SI_Image;// ������ ������ ȭ�鿡 ���̴� �̹���
    [SerializeField]
    private Text SI_Name; // ������ �̸�
    [SerializeField]
    private Text SI_LimitLvl;// ���� ����
    [SerializeField]
    private Text SI_DefaultStat;// �⺻ȿ��(�⺻���Ȱ���) ����
    [SerializeField]
    private GameObject SI_Panel;// ���þ����� �г�
    #endregion

    // IMoveable�� Spell���� ��ӹ޴´�.
    public IMoveable MyMoveable { get; set; }

    private Image icon;
    private Spell spell;
    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        // ���콺�� ���� �������� �̵��Ѵ�.
        // icon.transform.position = Input.mousePosition + offset;
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        Debug.Log("Take");
        // Ŭ���� ��ų ������ ������ Icon �� ��´�.
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;

    }

    public void SelectSpell(string spellName)
    {
        spell = SpellBook.MyInstance.GetSpell(spellName);
        selectName.text = spell.MyName;
        selectDescription.text = spell.MyDescription;
        selectImage.sprite = spell.MyIcon;
    }
    public void EquipSpell()
    {
        if (selectImage.sprite != null)
        {
            MyMoveable = spell;
            BlindControll();
        }
    }
    public void BlindControll() // ��ų ����Ҷ� ������ ����ȭ��
    {
        skillEquipping = !skillEquipping;
        if (!skillEquipping)
            MyMoveable = null;
        blindPanel.SetActive(skillEquipping);
    }
    public void ResetSelect()
    {
        selectName.text = null;
        selectDescription.text = null;
        selectImage.sprite = null;
        MyMoveable = null;
        skillEquipping = false;
        blindPanel.SetActive(false);
    }
    public void SelectItem(Item item) // ������ ����
    {
        myItem = item;
        SI_DefaultStat.text = "��� ȿ�� : "+myItem.MyEffect;
        SI_Image.sprite = myItem.MyIcon;
        SI_LimitLvl.text = "���� ���� : "+myItem.MyLimitLevel;
        SI_Name.text = ""+myItem.MyName;
        SI_Panel.SetActive(true);
    }
    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        // ������ ��ų ������ ������ null �� �����.
        MyMoveable = null;
        // ����� �������� �����ϰ� �����.
        icon.color = new Color(0, 0, 0, 0);
        // ������ ��ų�� ������ ������ �����Ѵ�.
        return tmp;
    }
    public void Drop()
    {
        // ������ ��ų ������ ������ null �� �����.
        MyMoveable = null;

        // Hand �������� �����ϰ� �����.
        icon.color = new Color(0, 0, 0, 0);
    }

}