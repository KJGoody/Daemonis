using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스팰 또는 아이템들의 클래스에서는 실행한느 함수만 구현하고 변수들을 저장하는 클래스를 만들어서 데이터 관리를한다.
// 이러게 변수를 저장하는 클래스를 데이터 테이블에 저장하여 데이터를 불러오는데 용이하게 한다.
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

    // 장비 아이템 데이터 테이블
    [SerializeField]
    private DataTable_Item_Equipment dataTable_Item_Equipment;
    public DataTable_Item_Equipment GetDataTable_Item_Equipment { get { return dataTable_Item_Equipment; } }

    // 소비 아이템 데이터 테이블
    [SerializeField]
    private DataTable_Item_Consumable dataTable_Item_Consumable;
    public DataTable_Item_Consumable GetDataTable_Item_Consumable { get { return dataTable_Item_Consumable; } }

    // 스팰 데이터 테이블
    [SerializeField]
    private DataTable_Spell dataTable_Spell;
    public DataTable_Spell GetDataTable_Spell { get { return dataTable_Spell; } }

    // 이름으로 장비 아이템 정보 가지고 오기
    public ItemInfo_Equipment GetItemInfo_Equipment(string Name)
    {
        foreach (DataArray_Item_Equipment Data in dataTable_Item_Equipment.Data_Item_Equipments)
            foreach (ItemInfo_Equipment ItemInfo in Data.items)
                if (Name == ItemInfo.itemName)
                    return ItemInfo;

        return null;
    }

    // 이름으로 소비 아이템 정보 가지고 오기
    public ItemInfo_Consumable GetItemInfo_Consumable(string Name)
    {
        foreach (DataArray_Item_Consumable Data in dataTable_Item_Consumable.Data_Item_Consumables)
            foreach (ItemInfo_Consumable ItemInfo in Data.items)
                if (Name == ItemInfo.itemName)
                    return ItemInfo;

        return null;
    }

    // 이름으로 스팰 정보 가지고 오기
    public SpellInfo GetSpellData(string Name)
    {
        foreach (DataArray_Spell Data in dataTable_Spell.Data_Spell)
            foreach (SpellInfo SpellInfo in Data.SpellInfos)
                if (Name == SpellInfo.SpellName)
                    return SpellInfo;

        return null;
    }
}