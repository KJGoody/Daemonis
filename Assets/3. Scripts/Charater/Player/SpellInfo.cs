using UnityEngine;

[System.Serializable]
public class SpellInfo
{
    public string ID;
    public string Name;
    public GameObject Prefab;
    public enum SpellType
    {
        #region 스킬 타입
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
    public string Description;     // 설명란
    public float CoolTime;
    public int ManaCost;
}
