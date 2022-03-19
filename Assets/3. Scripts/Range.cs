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


    // Player�� ������ ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.Target = collision.transform;
        }
    }

    // Player�� ������ ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.Target = null;
        }
    }

}