using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPortal : MonoBehaviour
{
    [HideInInspector]
    public string UnLoadSceneName;
    [HideInInspector]
    public string LoadSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActiveButton.Instance.SetButton(ActiveButton.Role.PortalButton, UnLoadSceneName, LoadSceneName);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ActiveButton.Instance.ResetButton();
    }
}