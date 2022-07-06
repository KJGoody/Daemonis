using UnityEngine;

[CreateAssetMenu(fileName = "SpellInfo")]
public class SpellInfo : ScriptableObject
{
    public GameObject spellPrefab;
    public Sprite SpellIcon;
    public string SpellName;
    public string SpellDescription;     // 설명란

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
    public SpellType spellType;

    public float SpellCoolTime;
    public int SpellMana;
}
