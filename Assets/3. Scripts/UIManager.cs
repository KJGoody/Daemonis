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
        Debug.Log(actionButtons[0]);
        SetUseable(actionButtons[0], SpellBook.MyInstance.GetSpell("FireBall"));
        //SetUseable(actionButtons[1], SpellBook.MyInstance.GetSpell("ShadowBall"));
        //SetUseable(actionButtons[2], SpellBook.MyInstance.GetSpell("FireBall"));
        //SetUseable(actionButtons[3], SpellBook.MyInstance.GetSpell("ShadowBall"));
        //SetUseable(actionButtons[4], SpellBook.MyInstance.GetSpell("FireBall"));

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

        // �������� UI�� ���ų� Ų��.
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        // UI �� Ŀ������ �� �����ɽ�Ʈ �浹�� �ǵ��� �����
        // UI �� �������� �� �����ɽ�Ʈ �浹�� ���õǾ� �ٸ� ����(�� ���� ��)��
        // �� �� �ְ� �����.
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? false : true;
    }


    public void SetUseable(ActionButton btn ,IUseable useable)
    {
        btn.MyIcon.sprite = useable.MyIcon;
        btn.MyIcon.color = Color.white;
        btn.MyUseable = useable;
    }
    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }


}
