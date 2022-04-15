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
    #region 아이템 선택관련 변수 SI = Select Item
    private Item myItem;    // 아이템 정보
    [SerializeField]
    private Image SI_Image;// 선택한 아이템 화면에 보이는 이미지
    [SerializeField]
    private Text SI_Name; // 아이템 이름
    [SerializeField]
    private Text SI_LimitLvl;// 제한 레벨
    [SerializeField]
    private Text SI_DefaultStat;// 기본효과(기본스탯같은) 설명
    [SerializeField]
    private GameObject SI_Panel;// 선택아이템 패널
    #endregion

    // IMoveable은 Spell에서 상속받는다.
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
    }
    public void EquipSpell()
    {
        if (selectImage.sprite != null)
        {
            MyMoveable = spell;
            BlindControll();
        }
    }
    public void BlindControll() // 스킬 등록할때 나오는 검은화면
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
    public void SelectItem(Item item) // 아이템 선택
    {
        myItem = item;
        SI_DefaultStat.text = "사용 효과 : "+myItem.MyEffect;
        SI_Image.sprite = myItem.MyIcon;
        SI_LimitLvl.text = "제한 레벨 : "+myItem.MyLimitLevel;
        SI_Name.text = ""+myItem.MyName;
        SI_Panel.SetActive(true);
    }
    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        // 복사한 스킬 아이콘 정보를 null 로 만든다.
        MyMoveable = null;
        // 복사된 아이콘을 투명하게 만든다.
        icon.color = new Color(0, 0, 0, 0);
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