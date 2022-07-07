using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableManager : MonoBehaviour
{
    private static DataTableManager instance;
    public static DataTableManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DataTableManager>();
            return instance;
        }
    }

    [SerializeField]
    private DataTable_Item_Equipment dataTable_Item_Equipment;
    public DataTable_Item_Equipment GetDataTable_Item_Equipment { get { return dataTable_Item_Equipment; } }

    [SerializeField]
    private DataTable_Item_Consumable dataTable_Item_Consumable;
    public DataTable_Item_Consumable GetDataTable_Item_Consumable { get { return dataTable_Item_Consumable; } }

    [SerializeField]
    private DataTable_Spell dataTable_Spell;
    public DataTable_Spell GetDataTable_Spell { get { return dataTable_Spell; } }

    public ItemInfo_Equipment GetItemInfo_Equipment(string Name)
    {
        foreach (DataArray_Item_Equipment Data in dataTable_Item_Equipment.Data_Item_Equipments)
            foreach (ItemInfo_Equipment ItemInfo in Data.items)
                if (Name == ItemInfo.itemName)
                    return ItemInfo;

        return null;
    }

    public ItemInfo_Consumable GetItemInfo_Consumable(string Name)
    {
        foreach (DataArray_Item_Consumable Data in dataTable_Item_Consumable.Data_Item_Consumables)
            foreach (ItemInfo_Consumable ItemInfo in Data.items)
                if (Name == ItemInfo.itemName)
                    return ItemInfo;

        return null;
    }

    public SpellInfo GetSpellData(string Name)
    {
        foreach (DataArray_Spell Data in dataTable_Spell.Data_Spell)
            foreach (SpellInfo SpellInfo in Data.SpellInfos)
                if (Name == SpellInfo.SpellName)
                    return SpellInfo;

        return null;
    }
}