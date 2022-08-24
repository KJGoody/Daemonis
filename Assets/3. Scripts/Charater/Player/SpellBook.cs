using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    // ΩÃ±€≈Ê
    private static SpellBook instance;
    public static SpellBook MyInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SpellBook>();

            return instance;
        }
    }
}
