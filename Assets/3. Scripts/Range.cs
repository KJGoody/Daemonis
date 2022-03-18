using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private Enemy parent;

    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }


    // Player가 들어왔을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.Target = collision.transform;
        }
    }

    // Player가 나갔을 때
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.Target = null;
        }
    }

}