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

    [SerializeField]
    private Spell[] spells; // Ω∫≈≥ ∏ÆΩ∫∆Æ

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        return spell;
    }
}
