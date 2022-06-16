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
    private CanvasGroup[] menu; // 메뉴창 0:캐릭터 1:스킬 2:인벤토리 3:옵션 4:메뉴리스트
    [SerializeField]
    private Image[] menuImage; //메뉴 이미지
    [SerializeField]
    private Sprite[] menuNormalImage; //메뉴 평소 이미지
    [SerializeField]
    private Sprite[] menuActiveImage; //메뉴 활성화 이미지
    [SerializeField]
    private GameObject tooltip;
    [SerializeField]
    private CanvasGroup bigMap;
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
    public void BigMapOnOff()
    {
        bigMap.alpha = bigMap.alpha > 0 ? 0 : 1;
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        // 투명값으로 UI를 끄거나 킨다.
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        switch (canvasGroup.name)
        {
            case "Charactor":
                if (canvasGroup.alpha == 1)
                    menuImage[0].sprite = menuActiveImage[0];
                else if(canvasGroup.alpha == 0)
                    menuImage[0].sprite = menuNormalImage[0];

                if (menu[2].alpha != 1 || menu[0].alpha != 1)
                    HandScript.MyInstance.Close_UE_Panel();
                break;

            case "SpellBook":
                if (canvasGroup.alpha == 1)
                    menuImage[1].sprite = menuActiveImage[1];
                else if (canvasGroup.alpha == 0)
                    menuImage[1].sprite = menuNormalImage[1];

                HandScript.MyInstance.ResetSelect();
                break;

            case "Inventory":
                if (canvasGroup.alpha == 1)
                    menuImage[2].sprite = menuActiveImage[2];
                else if (canvasGroup.alpha == 0)
                    menuImage[2].sprite = menuNormalImage[2];

                HandScript.MyInstance.Close_SI_Panel();
                if (menu[0].alpha != 1)
                    HandScript.MyInstance.Close_UE_Panel();
                break;

            case "Option":
                if (canvasGroup.alpha == 1)
                    menuImage[3].sprite = menuActiveImage[3];
                else if (canvasGroup.alpha == 0)
                    menuImage[3].sprite = menuNormalImage[3];
                break;

            case "MenuPanel":
                if (canvasGroup.alpha == 1)
                    menuImage[4].sprite = menuActiveImage[4];
                else if (canvasGroup.alpha == 0)
                    menuImage[4].sprite = menuNormalImage[4];
                break;
        }

        //if(canvasGroup.name == "SpellBook")
        //{
        //    HandScript.MyInstance.ResetSelect();
        //}
        //if (canvasGroup.name == "Inventory") 
        //{ 
        //    HandScript.MyInstance.Close_SI_Panel();
        //    if(menu[0].alpha != 1)
        //        HandScript.MyInstance.Close_UE_Panel();
        //}
        //if (canvasGroup.name == "Charactor")
        //    if(menu[2].alpha != 1 || menu[0].alpha != 1)
        //        HandScript.MyInstance.Close_UE_Panel();

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
