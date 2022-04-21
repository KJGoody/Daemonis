using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private EnemyBase parent;

    private void Start()
    {
        parent = GetComponentInParent<EnemyBase>();
    }


    // Player가 들어왔을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.SetTarget(collision.transform);
        }
    }
}