using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    
    [SerializeField]
    private Spell[] spells; // Ω∫≈≥ ∏ÆΩ∫∆Æ

    // ΩÃ±€≈Ê
    private static SpellBook instance;
    public static SpellBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }

            return instance;
        }
    }

    public Spell CastSpell(string spellName)
    {
        Spell spell =  Array.Find(spells, x => x.MyName == spellName); 
        
        return spell;
    }
    public Spell GetSpell(string spellName)
    {
        Spell spell =  Array.Find(spells, x => x.MyName == spellName); 
        
        return spell;
    }
}
