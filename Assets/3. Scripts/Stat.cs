using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public StatBar HealthBar;
    public float MaxHealth;

    private void Awake()
    {
        MaxHealth = 100f;
    }
}
