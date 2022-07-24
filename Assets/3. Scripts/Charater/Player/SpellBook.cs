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

    private DataArray_Spell[] SpellData;

    private void Start()
    {
        spells = new Spell[8];
        SpellData = DataTableManager.Instance.GetDataTable_Spell.Data_Spell;

        for(int i = 0; i < spells.Length; i++)
        {
            Spell Data = new Spell();
            Data.spellInfo = SpellData[0].SpellInfos[i];
            spells[i] = Data;
        }
    }

    public Spell GetSpell(string spellName) // 스킬 검색
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        return spell;
    }
}
