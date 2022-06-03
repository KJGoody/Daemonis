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

    [SerializeField]
    private Spell[] spells; // ��ų ����Ʈ

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        return spell;
    }
}
