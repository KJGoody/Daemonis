using System;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable
{
    [SerializeField]
    private string name;
    [SerializeField]
    private int damage;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject spellPrefab;
    [SerializeField]
    private string description;

    [SerializeField]
    private float castTime;
    [SerializeField]
    private Color barColor;


    public string MyName { get { return name; } }
    public int MyDamage { get { return damage; } }
    public Sprite MyIcon { get { return icon; } }
    public float MySpeed { get { return speed; } }

    public GameObject MySpellPrefab { get { return spellPrefab; } }
    public String MyDescription { get { return description; } }


    public float MyCastTime { get { return castTime; } }
    public Color MyBarColor { get { return barColor; } }


    public void Use()
    {
        Player.MyInstance.CastSpell(MyName);
    }
    public String GetName()
    {
        return MyName;
    }

}