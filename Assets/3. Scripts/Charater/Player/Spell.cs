using System;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable
{
    [SerializeField]
    private GameObject spellPrefab;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string name;
    [SerializeField]
    private string description;     // 설명란
    [SerializeField]
    private float SpellCoolTime;

    public enum SpellType
    {
        #region 스킬 타입
        Launch,
        Buff,
        AOE,
        Toggle,
        Immediate,
        AE
        #endregion
    }
    public SpellType spellType;

    public GameObject MySpellPrefab { get { return spellPrefab; } }
    public Sprite MyIcon { get { return icon; } }
    public string MyName { get { return name; } }
    public String MyDescription
    {
        get { return description; }
        set { description = value; }
    }
    public float MySpellCoolTime { get { return SpellCoolTime; } }


    public void Use()
    {
        if (spellType.Equals(SpellType.Buff))
            Player.MyInstance.NewBuff(MyName);
        else
            Player.MyInstance.CastSpell(MyName);
    }

    public String GetName()
    {
        return MyName;
    }

}