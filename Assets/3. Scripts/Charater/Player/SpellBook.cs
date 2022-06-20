using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    // 싱글톤
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
    private Spell[] spells; // 스킬 리스트

    public Spell GetSpell(string spellName) // 스킬 검색
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        return spell;
    }
}
