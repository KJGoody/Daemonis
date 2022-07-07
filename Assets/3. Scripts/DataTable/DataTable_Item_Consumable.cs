using UnityEngine;

[System.Serializable]
public class DataArray_Item_Consumable
{
    public ItemInfo_Consumable[] items;
}

[CreateAssetMenu(fileName = "DataTable_Item_Consumable", menuName = "DataTable/DataTable_Item_Consumable")]
public class DataTable_Item_Consumable : ScriptableObject
{
    public DataArray_Item_Consumable[] Data_Item_Consumables;
}
