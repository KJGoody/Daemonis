using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest123123123123123123123123 : MonoBehaviour
{
    public GameObject drop;
    public GameObject droppppppp;
    public GameObject Noticeeeeeeeee;
    public GameObject nPanel;
    public Item item;
   public void MMMMMMM()
    {
        Instantiate(drop, droppppppp.transform).GetComponent<DropItem>();
    }

    public void VVVVVV()
    {
        GameObject.Instantiate(Noticeeeeeeeee, new Vector3(0, 0, 0), Quaternion.identity).transform.SetParent(nPanel.transform);
    }
}
