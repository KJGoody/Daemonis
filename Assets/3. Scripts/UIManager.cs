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
    [SerializeField]
    private CanvasGroup spellBook;

    void Start()
    {

        //SetUseable(actionButtons[0], SpellBook.MyInstance.GetSpell("FireBall"));


    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(spellBook);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //InventoryScript.MyInstance.OpenClose();
        }
    }
    public void OpenClose(CanvasGroup canvasGroup)
    {
        Debug.Log(canvasGroup.name);

        // 투명값으로 UI를 끄거나 킨다.
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        if(canvasGroup.name == "SpellBook")
            HandScript.MyInstance.ResetSelect();
        // UI 가 커져있을 땐 레이케스트 충돌이 되도록 만들고
        // UI 가 꺼져있을 땐 레이케스트 충돌이 무시되어 다른 조작(적 선택 등)을
        // 할 수 있게 만든다.
        Debug.Log("1 = "+ !canvasGroup.blocksRaycasts);
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? false : true;
        Debug.Log("2 = " + canvasGroup.blocksRaycasts);
    }


    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount == 0)
        {
            // 해당 슬롯의 아이콘 투명하게 만들기
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
        }
    }

}
