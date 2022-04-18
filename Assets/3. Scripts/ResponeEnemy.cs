using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponeEnemy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SponeEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SponeEnemy()
    {
        while (true)
        {
            Instantiate(Resources.Load("Enemy/Enemy_BaseMeleeAttack") as GameObject, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }
}
