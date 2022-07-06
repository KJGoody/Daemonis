using UnityEngine;

[CreateAssetMenu(fileName = "SpellInfo")]
public class SpellInfo : ScriptableObject
{
    public GameObject spellPrefab;
    public Sprite SpellIcon;
    public string SpellName;
    public string SpellDescription;     // �����

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
    public SpellType spellType;

    public float SpellCoolTime;
    public int SpellMana;
}
