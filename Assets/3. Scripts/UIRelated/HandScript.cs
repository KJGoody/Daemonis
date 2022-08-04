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
    [SerializeField]
    private GameObject SpellEquipButton;
    #endregion

    public GameObject usingEquipment_Panel;
    public PlayerInfoPanel playerInfoPanel;
    private Item_Base myItem;    // ������ ����
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
    private Text SI_Descript;// ������ ��漳�� (������ �Ұ�)
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
    [SerializeField]
    private GameObject UE_Panel;// ���������� �г�

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
        icon.sprite = moveable.Icon;
        icon.color = Color.white;
    }

    public void SelectSpell(string spellName) // ��ų ����
    {
        spell = SpellBook.MyInstance.GetSpell(spellName); // �̸����� ��ų ã��

        switch (spell.Type) 
        {
            case SpellInfo.SpellType.Passive: // ��ų�� �нú��Ͻ� ��ư ��Ȱ��ȭ
                SpellEquipButton.SetActive(false);
                selectName.text = spell.Name + " (�нú�)";
                break;

            default:
                SpellEquipButton.SetActive(true);
                selectName.text = spell.Name;
                break;
        }

        // ��ų ����â�� ǥ��
        selectDescription.text = spell.Description;
        selectImage.sprite = spell.Icon;
        Color color = new Color(1, 1, 1, 1);
        selectImage.color = color;
    }

    public void EquipSpell() //��ų ����
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

    public void ResetSelect() // ���� ��ų �ʱ�ȭ
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
    public void SelectItem(Item_Base item) // ������ ����
    {
        // ������ ������ ���� ǥ��
        myItem = item;
        SI_Image.sprite = myItem.Icon;
        SI_Name.text = myItem.Name;
        SI_Quality.text = myItem.QualityText;
        SI_LimitLvl.text = "���� ���� : "+myItem.LimitLevel;
        SI_DefaultStat.text = "��� ȿ�� : "+myItem.Effect;
        SI_Descript.text = myItem.Descript;
        switch (myItem.Kind)
        {
            case ItemInfo_Base.Kinds.Potion: // ������ �������� ������ �� �߿�,��Ʈ�� ���߱�
                SI_Obj_Option.SetActive(false);
                SI_Obj_SetOption.SetActive(false);
                playerInfoPanel.ShowUsingEquipment(0, false);
                break;

            case ItemInfo_Base.Kinds.Equipment: // ������ �������� ����� �� �߿�, ��Ʈ�� ǥ��
                SI_Obj_Option.SetActive(true);

                for(int i = 0; i < (myItem as Item_Equipment).addOptionList.Count; i++) // �߿� ǥ��
                {
                    ItemAddOptionInfo optionInfo = SI_Obj_AddOptions[i].GetComponent<ItemAddOptionInfo>();
                    optionInfo.SetAddOptionPrefab((myItem as Item_Equipment).addOptionList[i]);
                    SI_Obj_AddOptions[i].SetActive(true);
                }
                for(int i = 6; i > (myItem as Item_Equipment).addOptionList.Count; i--)
                {
                    SI_Obj_AddOptions[i-1].SetActive(false);
                }
                SI_Obj_SetOption.SetActive(false); // ���߿� ��Ʈ��� ���ǹ����� Ȱ��ȭ

                // ��� ������ ���� �������� ��� ǥ��
                int partNum = (int)(item as Item_Equipment).GetPart;
                if(Player.MyInstance.usingEquipment[partNum] != null)
                {
                    playerInfoPanel.ShowUsingEquipment(partNum);
                }
                else
                {
                    playerInfoPanel.ShowUsingEquipment(partNum,false);
                }
                break;

            default:
                break;
        }
        SI_Panel.SetActive(true);

        // content size filtter �ٷ� �ȴþ�� ���� �ذ��
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)SI_CSF_Descript.transform); 
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)SI_CSF_Panel.transform);
    }

    public void EquipButton() // ������ư�� (�����̶� ��� ������ ����)
    {
        if (myItem.Kind == ItemInfo_Base.Kinds.Potion)
            EquipPotion();
        else if (myItem.Kind == ItemInfo_Base.Kinds.Equipment)
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
        int partNum = (int)(myItem as Item_Equipment).GetPart;
        if (Player.MyInstance.usingEquipment[partNum] != null)
        {
            Player.MyInstance.UnequipItem(partNum);
        }    
        myItem.Use();
        playerInfoPanel.ShowUsingEquipment(partNum,false);
        SI_Panel.SetActive(false);
    }

    public void ResetEquipPotion() // ���� ��� ���
    {
        MyMoveable = null;
        SI_Obj_Blind.SetActive(false);

    }

    public void RemoveItem() // ������ ���� ��ư
    {
        myItem.Remove();
        SI_Panel.SetActive(false);
        myItem = null;
    }

    public void _SellItem()
    {
        BuySellWindow.Instance.SetWindow(false, myItem);
    }

    public void Close_SI_Panel() // ���� ������ �г� �ݱ�
    {
        SI_Panel.SetActive(false);
    }

    public void Close_UE_Panel() // ���� ������ �г� �ݱ�
    {
        UE_Panel.SetActive(false);
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
            if (myItem.Kind == ItemInfo_Base.Kinds.Potion)
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