using UnityEngine;

[System.Serializable]
public class SpellInfo
{
    public string ID;
    public enum SpellType
    {
        #region ��ų Ÿ��
        Launch,
        Buff,
        AOE,
        Toggle,
        Target,
        AE,
        Passive,
        None
        #endregion
    }
    public SpellType Type;
    public GameObject Prefab;
    public Sprite Icon;
    public string Name;
    public string Description;
    public float CoolTime;
    public int ManaCost;
    public int Speed;
    public float SpellxDamage;
    public string Sound;
}
