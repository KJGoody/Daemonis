using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPortal : MonoBehaviour
{
    GameObject activeButton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        activeButton = GameObject.Find("Canvas").transform.Find("ActiveButton").gameObject;
        activeButton.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activeButton = GameObject.Find("Canvas").transform.Find("ActiveButton").gameObject;
        activeButton.SetActive(false);
    }
}