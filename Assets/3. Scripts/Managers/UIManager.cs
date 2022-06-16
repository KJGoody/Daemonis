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
    private CanvasGroup[] menu; // �޴�â 0:ĳ���� 1:��ų 2:�κ��丮 3:�ɼ� 4:�޴�����Ʈ
    [SerializeField]
    private Image[] menuImage; //�޴� �̹���
    [SerializeField]
    private Sprite[] menuNormalImage; //�޴� ��� �̹���
    [SerializeField]
    private Sprite[] menuActiveImage; //�޴� Ȱ��ȭ �̹���
    [SerializeField]
    private GameObject tooltip;
    [SerializeField]
    private CanvasGroup bigMap;
    private Text tooltipText;

    private void Awake()
    {
        // Tag �� keybind�� ������ ���ӿ�����Ʈ�� ã���ϴ�.
        //keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");

        // ������ ���� ����
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
        // �������� UI�� ���ų� Ų��.
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

        // UI �� Ŀ������ �� �����ɽ�Ʈ �浹�� �ǵ��� �����
        // UI �� �������� �� �����ɽ�Ʈ �浹�� ���õǾ� �ٸ� ����(�� ���� ��)��
        // �� �� �ְ� �����.
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? false : true;
        
    }

    // ƫ��UI Ȱ��ȭ
    public void ShowTooltip(Vector3 position, IDescribable description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;

        // �������� ������ �������ӿ�����Ʈ�� ����
        tooltipText.text = description.GetDescription();
    }

    // ƫ��UI ��Ȱ��ȭ
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
            // �ش� ������ ��ø���� ǥ���ϱ�
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }

        else
        {
            // �ش� ������ �ؽ�Ʈ �����ϰ� �����
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }

        if (clickable.MyCount == 0)
        {
            // �ش� ������ ������ �����ϰ� �����
            clickable.MyIcon.color = new Color(0, 0, 0, 0);

            // �ش� ������ �ؽ�Ʈ �����ϰ� �����
            clickable.MyStackText.color = new Color(0, 0, 0, 0);

        }
    }

    public void _LoadSceneName(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
