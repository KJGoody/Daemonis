using System;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable
{
    private SpellInfo Info;
    public string ID { get { return Info.ID; } }
    public void SetSpellInfo(SpellInfo spellInfo) { Info = spellInfo; }
    public SpellInfo.SpellType Type { get { return Info.Type; } }
    public GameObject Prefab { get { return Info.Prefab; } }
    public Sprite Icon { get { return Info.Icon; } }
    public string Name { get { return Info.Name; } }
    public string Description { get { return Info.Description; } }
    public float CoolTime { get { return Info.CoolTime; } }
    public int ManaCost { get { return Info.ManaCost; } }

    public string GetName() { return Name; }

    public void Use()
    {
        if (Info.Type.Equals(SpellInfo.SpellType.Buff))
            Player.MyInstance.NewBuff(Info.ID);
        else
            Player.MyInstance.CastSpell(Info.ID);
    }
}