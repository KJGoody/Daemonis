using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveButton : MonoBehaviour
{
    private static ActiveButton instance;
    public static ActiveButton Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ActiveButton>();
            return instance;
        }
    }

    [SerializeField] private CanvasGroup SetActive;
    [SerializeField] private Image Image;
    [SerializeField] private Sprite[] Images;

    public enum Role
    {
        PortalButton,
        MerchantButton,
        ChesterButton,
        QuesterButton
    }
    private Role RoleName;
    private bool IsQuestTalk;

    [HideInInspector] public string UnloadSceneName;
    [HideInInspector] public string LoadSceneName;

    public void SetButton(Role RoleName, bool isQuestTalk = false)
    {
        ResetButton();

        this.RoleName = RoleName;
        IsQuestTalk = isQuestTalk;
        switch (RoleName)
        {
            case Role.MerchantButton:
                Image.sprite = Images[1];
                break;

            case Role.ChesterButton:
                Image.sprite = Images[1];
                break;

            case Role.QuesterButton:
                Image.sprite = Images[1];
                break;

        }

        ButtonSetActive(true);
    }

    public void SetButton(Role RoleName, string UnloadSceneName, string LoadSceneName)
    {
        ResetButton();

        this.RoleName = RoleName;
        Image.sprite = Images[0];
        this.UnloadSceneName = UnloadSceneName;
        this.LoadSceneName = LoadSceneName;

        ButtonSetActive(true);
    }

    public void ResetButton()
    {
        IsQuestTalk = false;
        UnloadSceneName = null;
        LoadSceneName = null;

        ButtonSetActive(false);
    }

    public void _ClickButton()
    {
        switch (RoleName)
        {
            case Role.PortalButton:
                PortalManager.MyInstance._UnloadSceneName(UnloadSceneName);
                PortalManager.MyInstance._LoadSceneName(LoadSceneName);
                ResetButton();
                break;

            case Role.MerchantButton:
                if (IsQuestTalk)
                {
                    DialogScript.Instance.OpenDialog("Merchant");
                    ResetButton();
                }
                else
                    StorePanel.Instance.OpenStore();
                break;

            case Role.ChesterButton:
                ChestPanel.Instance.OpenChest();
                break;

            case Role.QuesterButton:
                if (IsQuestTalk)
                    DialogScript.Instance.OpenDialog("Quester");
                else
                    DialogScript.Instance.OpenDialog("Quester", (int)DialogData.QuestStats.Ing);

                ResetButton();
                break;
        }
    }

    public void ButtonSetActive(bool B)
    {
        if (B)
        {
            SetActive.alpha = 1;
            SetActive.blocksRaycasts = true;
        }
        else
        {
            SetActive.alpha = 0;
            SetActive.blocksRaycasts = false;
        }
    }
}
