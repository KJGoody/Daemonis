using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    public Image[] equipment_Img = new Image[6];
    public Sprite[] emtyImg = new Sprite[6];
    private int itemNum;
    private ItemBase ueItem;
    [Header("Using Equipment Tooltip")]
    #region ���� �������� ���� ���� UE = Using Equipment
    [SerializeField]
    private Image UE_Image; // ������ ������ ȭ�鿡 ���̴� �̹���
    [SerializeField]
    private Text UE_Name;   // ������ �̸�
    [SerializeField]
    private Text UE_LimitLvl;// ���� ����
    [SerializeField]
    private Text UE_DefaultStat;// �⺻ȿ��(�⺻���Ȱ���) ����
    [SerializeField]
    private Text UE_Descript;// ������ ���?���� (������ �Ұ�)
    [SerializeField]
    private Text UE_Quality;// ������ ���
    [SerializeField]
    private GameObject UE_Panel;// ���þ����� �г�
    [SerializeField]
    private GameObject UE_Obj_Option;// �߰��ɼ� ������Ʈ
    [SerializeField]
    private GameObject UE_Obj_SetOption;// ��Ʈ�ɼ� ������Ʈ
    //[SerializeField]
    //private GameObject UE_Obj_Blind;// ����ε� �г� ������Ʈ
    [SerializeField]
    private ContentSizeFitter UE_CSF_Descript;
    [SerializeField]
    private ContentSizeFitter UE_CSF_Panel;
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
    #endregion

    void Start()
    {
        Player.MyInstance.useEquipment += ChangeEquipment;
        initEquipment();
    }
    public void initEquipment()
    {
        for(int i = 0; i < equipment_Img.Length; i++)
        {
            ChangeEquipment(i);
        }
    }

    public void ChangeEquipment(int partNum)
    {
        if (Player.MyInstance.usingEquipment[partNum] != null)
        {
            equipment_Img[partNum].sprite = Player.MyInstance.usingEquipment[partNum].MyIcon;
            equipment_Img[partNum].color= new Color(1,1,1,1);
        }
        else
        {
            equipment_Img[partNum].sprite = emtyImg[partNum];
            equipment_Img[partNum].color= new Color(1,1,1,0.5f);
        }
    }
    public void UnequipButton()
    {
        Player.MyInstance.UnequipItem(itemNum);
        ChangeEquipment(itemNum);
    }
    public void ShowUsingEquipment(int partNum)
    {
        if(Player.MyInstance.usingEquipment[partNum] != null)
        {
            itemNum = partNum;
            ueItem = Player.MyInstance.usingEquipment[partNum];

            UE_Image.sprite = ueItem.MyIcon;
            UE_Name.text = ueItem.MyName;
            UE_Quality.text = ueItem.MyQualityText;
            UE_LimitLvl.text = "���� ���� : " + ueItem.MyLimitLevel;
            UE_DefaultStat.text = "��� ȿ�� : " + ueItem.MyEffect;
            UE_Descript.text = ueItem.MyDescript;
            switch (ueItem.GetKind)
            {
                case Kinds.Potion:
                    UE_Obj_Option.SetActive(false);
                    UE_Obj_SetOption.SetActive(false);
                    break;
                case Kinds.Equipment:
                    UE_Obj_Option.SetActive(true);
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
    void Update()
    {
        UpdateStat();
    }
    void UpdateStat()
    {
        Stat stat = Player.MyInstance.MyStat;
        ATK.text = "" + stat.CurrentAttack;
        HP.text = "" + stat.CurrentMaxHealth;
        MP.text = "" + stat.CurrentMaxMana;
        Def.text = "" + stat.CurrentDefence;
        mDef.text = "" + stat.CurrentMagicRegist;
        moveSpeed.text = "" + stat.MoveSpeedPercent + "%";
        atkSpeed.text = "" + stat.CurrentAttackSpeed;
        dodge.text = "" + stat.DodgePercent + "%";
        hitPercent.text = "" + stat.HitPercent + "%";
        criPercent.text = "" + stat.CriticalPercent + "%";
        criDamage.text = "" + stat.CriticalDamage + "%";
        hpRegen.text = "" + stat.HealthRegen;
        mpRegen.text = "" + stat.ManaRegen;
        onHitHp.text = "" + stat.RecoverHealth_onhit;
        onHitMp.text = "" + stat.RecoverMana_onhit;
        coolDown.text = "" + stat.CoolDown + "%";
        lootRange.text = "" + stat.ItemLootRangePercent + "%";
        dropPercent.text = "" + stat.ItemDropPercent + "%";
        goldPlus.text = "" + stat.GoldPlus + "%";
        expPlus.text = "" + stat.ExpPlus + "%";
        vampiric.text = "" + stat.VampiricRate + "%";
        stat.SetHpMP();
    }
}
