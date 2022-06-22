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
    #region 장착 장비아이템 관련 변수 UE = Using Equipment
    [SerializeField]
    private Image UE_Image; // 선택한 아이템 화면에 보이는 이미지
    [SerializeField]
    private Text UE_Name;   // 아이템 이름
    [SerializeField]
    private Text UE_LimitLvl;// 제한 레벨
    [SerializeField]
    private Text UE_DefaultStat;// 기본효과(기본스탯같은) 설명
    [SerializeField]
    private Text UE_Descript;// 아이템 배경?설명 (아이템 소개)
    [SerializeField]
    private Text UE_Quality;// 아이템 등급
    [SerializeField]
    private GameObject UE_Panel;// 선택아이템 패널
    [SerializeField]
    private GameObject UE_Obj_Option;// 추가옵션 오브젝트
    [SerializeField]
    private GameObject UE_Obj_SetOption;// 세트옵션 오브젝트
    [SerializeField]
    private GameObject[] UE_Obj_AddOptions;// 추가옵션들
    //[SerializeField]
    //private GameObject UE_Obj_Blind;// 블라인드 패널 오브젝트
    [SerializeField]
    private ContentSizeFitter UE_CSF_Descript;
    [SerializeField]
    private ContentSizeFitter UE_CSF_Panel;
    #endregion

    [Header("Stat Text")]
    #region 스탯창 텍스트
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
        for(int i = 0; i < equipment_Img.Length; i++)
        {
            ChangeEquipment(i);
        }
    }

    public void ChangeEquipment(int partNum) // 착용된 장비 이미지 교체
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
    public void UnequipButton() // 장착해제 버튼
    {
        Player.MyInstance.UnequipItem(itemNum);
        ChangeEquipment(itemNum);
    }
    public void ShowUsingEquipment(int partNum, bool setActive = true) // 인수 하나가 더 필요한 상황이 있어서 만든것
    {
        if(Player.MyInstance.usingEquipment[partNum] != null)
        {

            ShowUsingEquipment(partNum);// 기존 ShowUsingEquipment 기능은 같음
            UE_Panel.SetActive(setActive); // false인 경우 패널 닫아주는것
        }
    }
    public void ShowUsingEquipment(int partNum) // 장착중인 장비 표시
    {
        if (Player.MyInstance.usingEquipment[partNum] != null)
        {
            itemNum = partNum;
            ueItem = Player.MyInstance.usingEquipment[partNum];

            UE_Image.sprite = ueItem.MyIcon;
            UE_Name.text = ueItem.MyName;
            UE_Quality.text = ueItem.MyQualityText;
            UE_LimitLvl.text = "제한 레벨 : " + ueItem.MyLimitLevel;
            UE_DefaultStat.text = "사용 효과 : " + ueItem.MyEffect;
            UE_Descript.text = ueItem.MyDescript;
            switch (ueItem.GetKind)
            {
                case Kinds.Potion:
                    UE_Obj_Option.SetActive(false);
                    UE_Obj_SetOption.SetActive(false);
                    break;
                case Kinds.Equipment:
                    UE_Obj_Option.SetActive(true);

                    for (int i = 0; i < ueItem.addOptionList.Count; i++)
                    {
                        AddOptionInfo optionInfo = UE_Obj_AddOptions[i].GetComponent<AddOptionInfo>();
                        optionInfo.SetAddOptionPrefab(ueItem.addOptionList[i]);
                        UE_Obj_AddOptions[i].SetActive(true);
                    }
                    for (int i = 6; i > ueItem.addOptionList.Count; i--)
                    {
                        UE_Obj_AddOptions[i - 1].SetActive(false);
                    }

                    UE_Obj_SetOption.SetActive(false); // 나중에 세트장비 조건문으로 활성화
                    break;
                default:
                    break;
            }
            UE_Panel.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)UE_CSF_Descript.transform); // content size filtter 바로 안늘어나는 버그 해결
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)UE_CSF_Panel.transform);
        }
    }
    void Update()
    {
        UpdateStat();
    }
    void UpdateStat() // 스탯 표시
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
