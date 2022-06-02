using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    // 싱글톤
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
    #region 스킬 선택관련 변수
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
    private ItemBase myItem;    // 아이템 정보
    [Header ("Select Item Tooltip")]
    #region 아이템 선택관련 변수 SI = Select Item
    [SerializeField]
    private Image SI_Image; // 선택한 아이템 화면에 보이는 이미지
    [SerializeField]
    private Text SI_Name;   // 아이템 이름
    [SerializeField]
    private Text SI_LimitLvl;// 제한 레벨
    [SerializeField]
    private Text SI_DefaultStat;// 기본효과(기본스탯같은) 설명
    [SerializeField]
    private Text SI_Descript;// 아이템 배경?설명 (아이템 소개)
    [SerializeField]
    private Text SI_Quality;// 아이템 등급
    [SerializeField]
    private GameObject SI_Panel;// 선택아이템 패널
    [SerializeField]
    private GameObject SI_Obj_Option;// 추가옵션 오브젝트
    [SerializeField]
    private GameObject SI_Obj_SetOption;// 세트옵션 오브젝트
    [SerializeField]
    private GameObject SI_Obj_Blind;// 블라인드 패널 오브젝트
    [SerializeField]
    private GameObject[] SI_Obj_AddOptions;// 추가옵션들
    [SerializeField]
    private ContentSizeFitter SI_CSF_Descript;
    [SerializeField]
    private ContentSizeFitter SI_CSF_Panel;
    #endregion

    // IMoveable은 Spell에서 상속받는다.
    public IMoveable MyMoveable { get; set; }

    private Image icon;
    [SerializeField]
    private Vector3 offset; // 없어도됨

    private void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        // 마우스를 따라 아이콘이 이동한다.
        // icon.transform.position = Input.mousePosition + offset;
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        Debug.Log("Take");
        // 클릭한 스킬 아이콘 정보를 Icon 에 담는다.
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

    public void SkillBlindControll() // 스킬 등록할때 나오는 검은화면
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

    // 아이템 구현 함수 부분 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    public void SelectItem(ItemBase item) // 아이템 선택
    {
        myItem = item;
        SI_Image.sprite = myItem.MyIcon;
        SI_Name.text = myItem.MyName;
        SI_Quality.text = myItem.MyQualityText;
        SI_LimitLvl.text = "제한 레벨 : "+myItem.MyLimitLevel;
        SI_DefaultStat.text = "사용 효과 : "+myItem.MyEffect;
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

                SI_Obj_SetOption.SetActive(false); // 나중에 세트장비 조건문으로 활성화
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
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)SI_CSF_Descript.transform); // content size filtter 바로 안늘어나는 버그 해결
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)SI_CSF_Panel.transform);
    }

    public void EquipButton() // 장착버튼용 (포션이랑 장비 나누기 위함)
    {
        if (myItem.GetKind == Kinds.Potion)
            EquipPotion();
        else if (myItem.GetKind == Kinds.Equipment)
            UseEquipment();
        
    }
    public void EquipPotion() // 포션 등록 시작
    {
        MyMoveable = myItem;
        SI_Panel.SetActive(false);
        SI_Obj_Blind.SetActive(true);
    }
    public void UseEquipment() // 장비 장착할때
    {
        myItem.Use();
        SI_Panel.SetActive(false);
    }
    public void ResetEquipPotion() // 포션 등록 취소
    {
        MyMoveable = null;
        SI_Obj_Blind.SetActive(false);

    }
    public void Close_SI_Panel() // 선택 아이템 패널 닫기
    {
        SI_Panel.SetActive(false);
    }
    public IMoveable Put() // MyMoveable에 넣어져있는것 슬롯에 넣는 함수
    {
        IMoveable tmp = MyMoveable;
        // 복사한 스킬 아이콘 정보를 null 로 만든다.
        MyMoveable = null;
        // 복사된 아이콘을 투명하게 만든다.
        icon.color = new Color(0, 0, 0, 0);
        if (myItem != null)
        {
            if (myItem.GetKind == Kinds.Potion)
                HandScript.MyInstance.ResetEquipPotion(); // 이거만 포션 전용 코드
        }
        // 복사한 스킬의 아이콘 정보를 전달한다.
        return tmp;
    }
    public void Drop()
    {
        // 복사한 스킬 아이콘 정보를 null 로 만든다.
        MyMoveable = null;

        // Hand 아이콘을 투명하게 만든다.
        icon.color = new Color(0, 0, 0, 0);
    }

}