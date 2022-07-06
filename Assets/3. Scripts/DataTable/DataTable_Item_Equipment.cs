using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataArray_Item_Equipment
{
    public ItemInfo_Equipment[] items;
}

[CreateAssetMenu(fileName = "DataTable_Item_Equipment", menuName = "DataTable/DataTable_Item_Equipment")]
public class DataTable_Item_Equipment : ScriptableObject
{
    public DataArray_Item_Equipment[] Data_Item_Equipment;
}
