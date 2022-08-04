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

    private SpellInfo[] spellInfos;
    public SpellInfo[] SpellInfos { get { return spellInfos; } }

    private ItemInfo_Equipment[] equipmentInfos;
    public ItemInfo_Equipment[] EquipmentInfos { get { return equipmentInfos; } }

    [SerializeField]
    private DataTable_Item_Consumable dataTable_Item_Consumable;
    public DataTable_Item_Consumable GetDataTable_Item_Consumable { get { return dataTable_Item_Consumable; } }


    //public ItemInfo_Equipment GetItemInfo_Equipment(string Name)
    //{
    //    foreach (DataArray_Item_Equipment Data in dataTable_Item_Equipment.Data_Item_Equipments)
    //        foreach (ItemInfo_Equipment ItemInfo in Data.items)
    //            if (Name == ItemInfo.Name)
    //                return ItemInfo;

    //    return null;
    //}

    public ItemInfo_Consumable GetItemInfo_Consumable(string Name)
    {
        foreach (DataArray_Item_Consumable Data in dataTable_Item_Consumable.Data_Item_Consumables)
            foreach (ItemInfo_Consumable ItemInfo in Data.items)
                if (Name == ItemInfo.Name)
                    return ItemInfo;

        return null;
    }

    //public SpellInfo GetSpellData(string Name)
    //{
    //    foreach (DataArray_Spell Data in dataTable_Spell.Data_Spell)
    //        foreach (SpellInfo SpellInfo in Data.SpellInfos)
    //            if (Name == SpellInfo.Name)
    //                return SpellInfo;

    //    return null;
    //}

    private void Awake()
    {
        LoadDataTable_SpellInfo();
    }

    private void LoadDataTable_SpellInfo()
    {
        // 스팰 데이터 테이블을 불러오기
        List<Dictionary<string, object>> DataTable_Spell = CSVReader.Read("DataTable_Spell");
        // 데이터 테이블의 크기로 
        spellInfos = new SpellInfo[DataTable_Spell.Count];

        for (int i = 0; i < DataTable_Spell.Count; i++)
        {
            // SpellInfo를 생성하여 데이터를 입력한다.
            SpellInfo info = new SpellInfo();
            info.ID = DataTable_Spell[i]["ID"].ToString();
            switch (DataTable_Spell[i]["Type"].ToString())
            {
                #region 스팰 타입
                case "Launch":
                    info.Type = SpellInfo.SpellType.Launch;
                    break;
                case "Buff":
                    info.Type = SpellInfo.SpellType.Buff;
                    break;
                case "AOE":
                    info.Type = SpellInfo.SpellType.AOE;
                    break;
                case "Toggle":
                    info.Type = SpellInfo.SpellType.Toggle;
                    break;
                case "Immediate":
                    info.Type = SpellInfo.SpellType.Immediate;
                    break;
                case "AE":
                    info.Type = SpellInfo.SpellType.AE;
                    break;
                case "Passive":
                    info.Type = SpellInfo.SpellType.Passive;
                    break;
                case "None":
                    info.Type = SpellInfo.SpellType.None;
                    break;
                    #endregion
            }
            info.Prefab = Resources.Load<GameObject>("Prefabs/" + DataTable_Spell[i]["Prefab"].ToString());
            info.Icon = Resources.Load<Sprite>("Sprites/" + DataTable_Spell[i]["Icon"].ToString());
            info.Name = DataTable_Spell[i]["Name"].ToString();
            info.Description = DataTable_Spell[i]["Description"].ToString();
            info.CoolTime = float.Parse(DataTable_Spell[i]["CoolTime"].ToString());
            info.ManaCost = int.Parse(DataTable_Spell[i]["ManaCost"].ToString());

            spellInfos[i] = info;
        }
    }

    private void LoadDataTable_EquipmentInfo()
    {
        List<Dictionary<string, object>> DataTable_Spell = CSVReader.Read("DataTable_Spell");
    }
}