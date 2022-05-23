using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponeEnemy : MonoBehaviour
{
    [SerializeField]
    private float ResponeTime;

    void Start()
    {
        StartCoroutine(SponeEnemy());
    }

    IEnumerator SponeEnemy()
    {
        while (true)
        {
            Instantiate(Resources.Load("Enemy/EnemyBase") as GameObject, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(ResponeTime);
        }
    }
}
