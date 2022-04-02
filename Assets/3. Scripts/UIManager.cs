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


    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount == 0)
        {
            // �ش� ������ ������ �����ϰ� �����
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
        }
    }

}
