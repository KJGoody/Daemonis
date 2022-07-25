using UnityEngine;

[System.Serializable]
public class SpellInfo
{
    public string ID;
    public string Name;
    public GameObject Prefab;
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
    public Sprite Icon;
    public string Description;     // �����
    public float CoolTime;
    public int ManaCost;
}
