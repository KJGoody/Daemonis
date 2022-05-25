using System;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string name;
    [SerializeField]
    private string description;     // 설명란
    public enum SpellType
    {
        Launch,
        Buff,
        AOE,
        Toggle,
        AE
    }
    public SpellType spellType;
    [SerializeField]
    private GameObject spellPrefab;


    public string MyName { get { return name; } }
    public Sprite MyIcon { get { return icon; } }

    public GameObject MySpellPrefab { get { return spellPrefab; } }
    public String MyDescription { get { return description; } }


    public void Use()
    {
        if (spellType == SpellType.Buff)
            Player.MyInstance.NewBuff(MyName);
        else
            Player.MyInstance.CastSpell(MyName);
    }
    public String GetName()
    {
        return MyName;
    }

}