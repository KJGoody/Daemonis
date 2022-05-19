using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbingRange : MonoBehaviour
{
    private EnemyBase parent;

    private void Start()
    {
        parent = GetComponentInParent<EnemyBase>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            parent.RubbingTime += Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            parent.RubbingTime = 0f;
    }
}
