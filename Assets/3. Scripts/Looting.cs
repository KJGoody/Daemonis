using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            DropItem D_item = collision.GetComponent<DropItem>();
            D_item.L_Start = true;
        }
    }
}
