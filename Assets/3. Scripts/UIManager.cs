using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private ActionButton[] actionButtons;

    void Start()
    {
        Debug.Log(actionButtons[0]);
        SetUseable(actionButtons[0], SpellBook.MyInstance.GetSpell("FireBall"));
        //SetUseable(actionButtons[1], SpellBook.MyInstance.GetSpell("ShadowBall"));
        //SetUseable(actionButtons[2], SpellBook.MyInstance.GetSpell("FireBall"));
        //SetUseable(actionButtons[3], SpellBook.MyInstance.GetSpell("ShadowBall"));
        //SetUseable(actionButtons[4], SpellBook.MyInstance.GetSpell("FireBall"));

    }


    void Update()
    {

    }

    public void SetUseable(ActionButton btn ,IUseable useable)
    {
        btn.MyButton.image.sprite = useable.MyIcon;
        btn.MyButton.image.color = Color.white;
        btn.MyUseable = useable;
    }
    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }


}
