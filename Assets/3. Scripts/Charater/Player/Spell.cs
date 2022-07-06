using System;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable
{
    [SerializeField]
    private SpellInfo spellInfo;

    public GameObject MySpellPrefab { get { return spellInfo.spellPrefab; } }
    public Sprite MyIcon { get { return spellInfo.SpellIcon; } }
    public string MyName { get { return spellInfo.SpellName; } }
    public String MyDescription
    {
        get { return spellInfo.SpellDescription; }
        set { spellInfo.SpellDescription = value; }
    }
    public SpellInfo.SpellType spellType { get { return spellInfo.spellType; } }
    public float MySpellCoolTime { get { return spellInfo.SpellCoolTime; } }
    public int MySpellMana { get { return spellInfo.SpellMana; } }


    public void Use()
    {
        if (spellInfo.spellType.Equals(SpellInfo.SpellType.Buff))
            Player.MyInstance.NewBuff(MyName);
        else
            Player.MyInstance.CastSpell(MyName);
    }

    public String GetName()
    {
        return MyName;
    }

}