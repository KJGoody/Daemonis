using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalManager.MyInstance.ShowTeleportList();
    }
}
