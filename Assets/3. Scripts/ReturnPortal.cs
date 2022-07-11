using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPortal : MonoBehaviour
{
    ActiveButton activeButton;

    [HideInInspector]
    public string UnLoadSceneName;
    [HideInInspector]
    public string LoadSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        activeButton = GameObject.Find("Canvas").transform.Find("ActiveButton").gameObject.GetComponent<ActiveButton>();
        activeButton.SetButton(ActiveButton.Role.PortalButton, UnLoadSceneName, LoadSceneName);
        //activeButton.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activeButton = GameObject.Find("Canvas").transform.Find("ActiveButton").gameObject.GetComponent<ActiveButton>();
        activeButton.ResetButton();
        //activeButton.SetActive(false);
    }
}