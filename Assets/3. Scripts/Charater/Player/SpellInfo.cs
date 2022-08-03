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
        Immediate,
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
}
