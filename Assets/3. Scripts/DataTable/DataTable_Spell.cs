using UnityEngine;

[System.Serializable]
public class DataArray_Spell
{
    public SpellInfo[] SpellInfos;
}

[CreateAssetMenu(fileName = "DataTable_Spell", menuName = "DataTable/DataTable_Spell")]
public class DataTable_Spell : ScriptableObject
{
    public DataArray_Spell[] Data_Spell;
}