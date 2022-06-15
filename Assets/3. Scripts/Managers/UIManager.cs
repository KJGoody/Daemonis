using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private CanvasGroup[] menu; // 컴퓨터 키보드 호환용
    [SerializeField]
    private GameObject tooltip;
    private Text tooltipText;

    private void Awake()
    {
        // Tag 가 keybind로 설정된 게임오브젝트를 찾습니다.
        //keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");

        // 아이템 툴팁 참조
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenClose(menu[0]);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            OpenClose(menu[1]);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenClose(menu[2]);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenClose(menu[3]);
        }
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        // 투명값으로 UI를 끄거나 킨다.
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        if(canvasGroup.name == "SpellBook")
        {
            HandScript.MyInstance.ResetSelect();
        }
        if (canvasGroup.name == "Inventory") 
        { 
            HandScript.MyInstance.Close_SI_Panel();
            if(menu[0].alpha != 1)
                HandScript.MyInstance.Close_UE_Panel();
        }
        if (canvasGroup.name == "Charactor")
            if(menu[2].alpha != 1 || menu[0].alpha != 1)
                HandScript.MyInstance.Close_UE_Panel();
        // UI 가 커져있을 땐 레이케스트 충돌이 되도록 만들고
        // UI 가 꺼져있을 땐 레이케스트 충돌이 무시되어 다른 조작(적 선택 등)을
        // 할 수 있게 만든다.
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? false : true;
        
    }

    // 튤팁UI 활성화
    public void ShowTooltip(Vector3 position, IDescribable description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;

        // 아이템의 내용을 툴팁게임오브젝트에 전달
        tooltipText.text = description.GetDescription();
    }

    // 튤팁UI 비활성화
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            // 해당 슬롯의 중첩개수 표시하기
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }

        else
        {
            // 해당 슬롯의 텍스트 투명하게 만들기
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }

        if (clickable.MyCount == 0)
        {
            // 해당 슬롯의 아이콘 투명하게 만들기
            clickable.MyIcon.color = new Color(0, 0, 0, 0);

            // 해당 슬롯의 텍스트 투명하게 만들기
            clickable.MyStackText.color = new Color(0, 0, 0, 0);

        }
    }

    public void _LoadSceneName(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
