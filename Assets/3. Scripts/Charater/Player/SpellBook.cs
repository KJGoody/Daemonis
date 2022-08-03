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

    private Spell[] spells; // 스킬 리스트

    private void Start()
    {
        spells = new Spell[8];

        for(int i = 0; i < spells.Length; i++)
        {
            Spell Data = new Spell();
            Data.SetSpellInfo(DataTableManager.Instance.SpellInfos[i]);
            spells[i] = Data;
        }
    }

    public Spell GetSpell(string spellName) // 스킬 검색
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);

        return spell;
    }
}
