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
    [SerializeField]
    private GameObject tooltip;
    private Text tooltipText;
    private void Awake()
    {
        // Tag �� keybind�� ������ ���ӿ�����Ʈ�� ã���ϴ�.
        //keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");


        // ������ ���� ����
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }
    void Start()
    {

        


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

        // �������� UI�� ���ų� Ų��.
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        if(canvasGroup.name == "SpellBook")
            HandScript.MyInstance.ResetSelect();
        // UI �� Ŀ������ �� �����ɽ�Ʈ �浹�� �ǵ��� �����
        // UI �� �������� �� �����ɽ�Ʈ �浹�� ���õǾ� �ٸ� ����(�� ���� ��)��
        // �� �� �ְ� �����.
        Debug.Log("1 = "+ !canvasGroup.blocksRaycasts);
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? false : true;
        Debug.Log("2 = " + canvasGroup.blocksRaycasts);
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

}
