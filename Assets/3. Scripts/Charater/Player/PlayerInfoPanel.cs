using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    #region Instance
    private static PlayerInfoPanel instance;
    public static PlayerInfoPanel Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerInfoPanel>();
            return instance;
        }
    }
    #endregion
    public Image[] equipment_Img = new Image[6];
    public Sprite[] emtyImg = new Sprite[6];
    private int itemNum;
    private Item_Base ueItem;

    [Header("Using Equipment Tooltip")]
    #region ���� �������� ���� ���� UE = Using Equipment
    [SerializeField] private Image UE_Image; // ������ ������ ȭ�鿡 ���̴� �̹���
    [SerializeField] private Text UE_Name;   // ������ �̸�
    [SerializeField] private Text UE_LimitLvl;// ���� ����
    [SerializeField] private Text UE_DefaultStat;// �⺻ȿ��(�⺻���Ȱ���) ����
    [SerializeField] private Text UE_Descript;// ������ ���?���� (������ �Ұ�)
    [SerializeField] private Text UE_Quality;// ������ ���
    [SerializeField] private GameObject UE_Panel;// ���þ����� �г�
    [SerializeField] private GameObject UE_Obj_Option;// �߰��ɼ� ������Ʈ
    [SerializeField] private GameObject UE_Obj_SetOption;// ��Ʈ�ɼ� ������Ʈ
    [SerializeField] private GameObject[] UE_Obj_AddOptions;// �߰��ɼǵ�
    //[SerializeField]
    //private GameObject UE_Obj_Blind;// ����ε� �г� ������Ʈ
    [SerializeField] private ContentSizeFitter UE_CSF_Descript;
    [SerializeField] private ContentSizeFitter UE_CSF_Panel;
    #endregion

    [Header("Stat Text")]
    #region ����â �ؽ�Ʈ
    public Text ATK;
    public Text HP;
    public Text MP;
    public Text Def;
    public Text mDef;
    public Text moveSpeed;
    public Text atkSpeed;
    public Text dodge;
    public Text hitPercent;
    public Text criPercent;
    public Text criDamage;
    public Text hpRegen;
    public Text mpRegen;
    public Text onHitHp;
    public Text onHitMp;
    public Text coolDown;
    public Text lootRange;
    public Text dropPercent;
    public Text goldPlus;
    public Text expPlus;
    public Text vampiric;
    public Text level_UI;
    public Text gold_Inventory;
    #endregion

    void Start()
    {
        Player.MyInstance.useEquipment += ChangeEquipment;
        initEquipment();
    }

    public void initEquipment()
    {
        for (int i = 0; i < equipment_Img.Length; i++)
            ChangeEquipment(i);
    }

    public void ChangeEquipment(int partNum) // ����� ��� �̹��� ��ü
    {
        if (Player.MyInstance.usingEquipment[partNum] != null)
        {
            equipment_Img[partNum].sprite = Player.MyInstance.usingEquipment[partNum].Icon;
            equipment_Img[partNum].color = new Color(1, 1, 1, 1);
        }
        else
        {
            equipment_Img[partNum].sprite = emtyImg[partNum];
            equipment_Img[partNum].color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void UnequipButton() // �������� ��ư
    {
        Player.MyInstance.UnequipItem(itemNum);
        ChangeEquipment(itemNum);
    }

    public void ShowUsingEquipment(int partNum) // �������� ��� ǥ��
    {
        if (Player.MyInstance.usingEquipment[partNum] != null)
        {
            itemNum = partNum;
            ueItem = Player.MyInstance.usingEquipment[partNum];

            UE_Image.sprite = ueItem.Icon;
            UE_Name.text = ueItem.Name;
            UE_Quality.text = ueItem.QualityText;
            UE_LimitLvl.text = "���� ���� : " + ueItem.LimitLevel;
            UE_DefaultStat.text = "��� ȿ�� : " + ueItem.Effect;
            UE_Descript.text = ueItem.Descript;
            switch (ueItem.Kind)
            {
                case ItemInfo_Base.Kinds.Potion:
                    UE_Obj_Option.SetActive(false);
                    UE_Obj_SetOption.SetActive(false);
                    break;

                case ItemInfo_Base.Kinds.Equipment:
                    UE_Obj_Option.SetActive(true);

                    for (int i = 0; i < (ueItem as Item_Equipment).addOptionList.Count; i++)
                    {
                        ItemAddOptionInfo optionInfo = UE_Obj_AddOptions[i].GetComponent<ItemAddOptionInfo>();
                        optionInfo.SetAddOptionPrefab((ueItem as Item_Equipment).addOptionList[i]);
                        UE_Obj_AddOptions[i].SetActive(true);
                    }
                    for (int i = 6; i > (ueItem as Item_Equipment).addOptionList.Count; i--)
                    {
                        UE_Obj_AddOptions[i - 1].SetActive(false);
                    }

                    UE_Obj_SetOption.SetActive(false); // ���߿� ��Ʈ��� ���ǹ����� Ȱ��ȭ
                    break;
                default:
                    break;
            }
            UE_Panel.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)UE_CSF_Descript.transform); // content size filtter �ٷ� �ȴþ�� ���� �ذ�
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)UE_CSF_Panel.transform);
        }
    }

    public void Close_UE_Panel()
    {
        UE_Panel.SetActive(false);
    }

    void Update()
    {
        UpdateStat();
    }

    void UpdateStat() // ���� ǥ��
    {
        Stat stat = Player.MyInstance.MyStat;
        ATK.text = "" + stat.CurrentAttack;
        HP.text = "" + stat.CurrentMaxHealth;
        MP.text = "" + stat.CurrentMaxMana;
        Def.text = "" + stat.CurrentDefence;
        mDef.text = "" + stat.CurrentMagicRegist;
        moveSpeed.text = "" + stat.MoveSpeedPercent.ToString("F2") + "%";
        atkSpeed.text = "" + stat.CurrentAttackSpeed;
        dodge.text = "" + stat.DodgePercent.ToString("F2") + "%";
        hitPercent.text = "" + stat.HitPercent.ToString("F2") + "%";
        criPercent.text = "" + stat.CriticalPercent.ToString("F2") + "%";
        criDamage.text = "" + stat.CriticalDamage.ToString("F2") + "%";
        hpRegen.text = "" + stat.HealthRegen;
        mpRegen.text = "" + stat.ManaRegen;
        onHitHp.text = "" + stat.RecoverHealth_onhit;
        onHitMp.text = "" + stat.RecoverMana_onhit;
        coolDown.text = "" + stat.CoolDown.ToString("F2") + "%";
        lootRange.text = "" + stat.ItemLootRangePercent.ToString("F2") + "%";
        dropPercent.text = "" + stat.ItemDropPercent.ToString("F2") + "%";
        goldPlus.text = "" + stat.GoldPlus.ToString("F2") + "%";
        expPlus.text = "" + stat.ExpPlus.ToString("F2") + "%";
        vampiric.text = "" + stat.VampiricRate.ToString("F2") + "%";
        level_UI.text = "" + stat.Level;
        gold_Inventory.text = "" + GameManager.MyInstance.DATA.Gold;
        stat.SetHpMP();
    }
}
