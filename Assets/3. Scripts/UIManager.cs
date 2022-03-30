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
    }
    public void OpenClose(CanvasGroup canvasGroup)
    {
        Debug.Log(canvasGroup.name);

        // 투명값으로 UI를 끄거나 킨다.
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        // UI 가 커져있을 땐 레이케스트 충돌이 되도록 만들고
        // UI 가 꺼져있을 땐 레이케스트 충돌이 무시되어 다른 조작(적 선택 등)을
        // 할 수 있게 만든다.
        Debug.Log(canvasGroup.blocksRaycasts);
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? true : false;
    }


    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }


}
