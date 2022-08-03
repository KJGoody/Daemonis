using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    // �̱���
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

    private Spell[] spells; // ��ų ����Ʈ

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

    public Spell GetSpell(string spellName) // ��ų �˻�
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);

        return spell;
    }
}
