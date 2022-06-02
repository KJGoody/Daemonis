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

    private Spell spell;
    [Header ("Select Skill")]
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

    public GameObject usingEquipment_Panel;
    public PlayerInfoPanel playerInfoPanel;
    private ItemBase myItem;    // ������ ����
    [Header ("Select Item Tooltip")]
    #region ������ ���ð��� ���� SI = Select Item
    [SerializeField]
    private Image SI_Image; // ������ ������ ȭ�鿡 ���̴� �̹���
    [SerializeField]
    private Text SI_Name;   // ������ �̸�
    [SerializeField]
    private Text SI_LimitLvl;// ���� ����
    [SerializeField]
    private Text SI_DefaultStat;// �⺻ȿ��(�⺻���Ȱ���) ����
    [SerializeField]
    private Text SI_Descript;// ������ ���?���� (������ �Ұ�)
    [SerializeField]
    private Text SI_Quality;// ������ ���
    [SerializeField]
    private GameObject SI_Panel;// ���þ����� �г�
    [SerializeField]
    private GameObject SI_Obj_Option;// �߰��ɼ� ������Ʈ
    [SerializeField]
    private GameObject SI_Obj_SetOption;// ��Ʈ�ɼ� ������Ʈ
    [SerializeField]
    private GameObject SI_Obj_Blind;// ����ε� �г� ������Ʈ
    [SerializeField]
    private GameObject[] SI_Obj_AddOptions;// �߰��ɼǵ�
    [SerializeField]
    private ContentSizeFitter SI_CSF_Descript;
    [SerializeField]
    private ContentSizeFitter SI_CSF_Panel;
    #endregion

    // IMoveable�� Spell���� ��ӹ޴´�.
    public IMoveable MyMoveable { get; set; }

    private Image icon;
    [SerializeField]
    private Vector3 offset; // �����

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
        Color color = new Color(1, 1, 1, 1);
        selectImage.color = color;
    }

    public void EquipSpell()
    {
        if (selectImage.sprite != null)
        {
            MyMoveable = spell;
            SkillBlindControll();
        }
    }

    public void SkillBlindControll() // ��ų ����Ҷ� ������ ����ȭ��
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
        Color color = new Color(0, 0, 0, 0);
        selectImage.color = color;
        MyMoveable = null;
        skillEquipping = false;
        blindPanel.SetActive(false);
    }

    // ������ ���� �Լ� �κ� @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public void SelectItem(ItemBase item) // ������ ����
    {
        myItem = item;
        SI_Image.sprite = myItem.MyIcon;
        SI_Name.text = myItem.MyName;
        SI_Quality.text = myItem.MyQualityText;
        SI_LimitLvl.text = "���� ���� : "+myItem.MyLimitLevel;
        SI_DefaultStat.text = "��� ȿ�� : "+myItem.MyEffect;
        SI_Descript.text = myItem.MyDescript;
        switch (myItem.GetKind)
        {
            case Kinds.Potion:
                SI_Obj_Option.SetActive(false);
                SI_Obj_SetOption.SetActive(false);
                break;
            case Kinds.Equipment:
                SI_Obj_Option.SetActive(true);

                for(int i = 0; i < myItem.addOptionList.Count; i++)
                {
                    AddOptionInfo optionInfo = SI_Obj_AddOptions[i].GetComponent<AddOptionInfo>();
                    optionInfo.SetAddOptionPrefab(myItem.addOptionList[i]);
                    SI_Obj_AddOptions[i].SetActive(true);
                }
                for(int i = 6; i > myItem.addOptionList.Count; i--)
                {
                    SI_Obj_AddOptions[i-1].SetActive(false);
                }

                SI_Obj_SetOption.SetActive(false); // ���߿� ��Ʈ��� ���ǹ����� Ȱ��ȭ
                int partNum = (int)item.GetPart;
                if(Player.MyInstance.usingEquipment[partNum] != null)
                {
                    playerInfoPanel.ShowUsingEquipment(partNum);
                }
                break;

            default:
                break;
        }
        SI_Panel.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)SI_CSF_Descript.transform); // content size filtter �ٷ� �ȴþ�� ���� �ذ�
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)SI_CSF_Panel.transform);
    }

    public void EquipButton() // ������ư�� (�����̶� ��� ������ ����)
    {
        if (myItem.GetKind == Kinds.Potion)
            EquipPotion();
        else if (myItem.GetKind == Kinds.Equipment)
            UseEquipment();
        
    }
    public void EquipPotion() // ���� ��� ����
    {
        MyMoveable = myItem;
        SI_Panel.SetActive(false);
        SI_Obj_Blind.SetActive(true);
    }
    public void UseEquipment() // ��� �����Ҷ�
    {
        myItem.Use();
        SI_Panel.SetActive(false);
    }
    public void ResetEquipPotion() // ���� ��� ���
    {
        MyMoveable = null;
        SI_Obj_Blind.SetActive(false);

    }
    public void Close_SI_Panel() // ���� ������ �г� �ݱ�
    {
        SI_Panel.SetActive(false);
    }
    public IMoveable Put() // MyMoveable�� �־����ִ°� ���Կ� �ִ� �Լ�
    {
        IMoveable tmp = MyMoveable;
        // ������ ��ų ������ ������ null �� �����.
        MyMoveable = null;
        // ����� �������� �����ϰ� �����.
        icon.color = new Color(0, 0, 0, 0);
        if (myItem != null)
        {
            if (myItem.GetKind == Kinds.Potion)
                HandScript.MyInstance.ResetEquipPotion(); // �̰Ÿ� ���� ���� �ڵ�
        }
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