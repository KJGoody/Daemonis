using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveButton : MonoBehaviour
{
    [SerializeField]
    private Image Image;
    [SerializeField]
    private Sprite[] Images;

    public enum Role
    {
        PortalButton,
        MerchantButton
    }
    private Role RoleName;

    public delegate void ClickButton();
    public event ClickButton Click;

    [HideInInspector]
    public string UnloadSceneName;
    [HideInInspector]
    public string LoadSceneName;

    public delegate void ClickButton_Portal(string SceneName);
    public event ClickButton_Portal UnLoadScene;
    public event ClickButton_Portal LoadScene;

    public void SetButton(Role RoleName, string UnloadSceneName = null, string LoadSceneName = null)
    {
        ResetButton();

        this.RoleName = RoleName;
        switch (RoleName)
        {
            case Role.PortalButton:
                Image.sprite = Images[0];
                this.UnloadSceneName = UnloadSceneName;
                this.LoadSceneName = LoadSceneName;
                UnLoadScene = PortalManager.MyInstance._UnloadSceneName;
                LoadScene = PortalManager.MyInstance._LoadSceneName;
                break;

            case Role.MerchantButton:
                Image.sprite = Images[1];
                break;
        }

        gameObject.SetActive(true);
    }

    public void ResetButton(bool Set = false)
    {
        Click = null;

        UnLoadScene = null;
        LoadScene = null;
        UnloadSceneName = null;
        LoadSceneName = null;

        gameObject.SetActive(Set);
    }

    public void _ClickButton()
    {
        switch (RoleName)
        {
            case Role.PortalButton:
                UnLoadScene(UnloadSceneName);
                LoadScene(LoadSceneName);
                ResetButton();
                break;

            case Role.MerchantButton:
                Click();
                break;
        }
    }
}
