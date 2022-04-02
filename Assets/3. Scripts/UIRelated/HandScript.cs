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
    [SerializeField]
    private Text selectName;
    [SerializeField]
    private Image selectImgae;
    [SerializeField]
    private Text selectDescription;
    [SerializeField]
    private GameObject blindPanel;
    private bool skillEquipping = false;

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
        // ������ ��ų ������ ������ null �� �����.
        MyMoveable=null;
        // ����� �������� �����ϰ� �����.
        icon.color=new Color(0,0,0,0);
        // ������ ��ų�� ������ ������ �����Ѵ�.
        return tmp;
    }
    
}