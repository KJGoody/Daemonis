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
    [SerializeField]
    private Text selectName;
    [SerializeField]
    private Image selectImgae;
    [SerializeField]
    private Text selectDescription;
    [SerializeField]
    private GameObject blindPanel;
    private bool skillEquipping = false;

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
        selectImgae.sprite = spell.MyIcon;
    }
    public void EquipSpell()
    {
        if (selectImgae.sprite != null)
        {
            MyMoveable = spell;
            BlindControll();
        }
    }
    public void BlindControll()
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
        selectImgae.sprite = null;
        MyMoveable = null;
        skillEquipping = false;
        blindPanel.SetActive(false);
    }

    public IMoveable Put()
    {
        IMoveable tmp=MyMoveable;
        // 복사한 스킬 아이콘 정보를 null 로 만든다.
        MyMoveable=null;
        // 복사된 아이콘을 투명하게 만든다.
        icon.color=new Color(0,0,0,0);
        // 복사한 스킬의 아이콘 정보를 전달한다.
        return tmp;
    }
    
}