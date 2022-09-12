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
    private ItemInfo_Equipment[] EquipmentInfos;
    private ItemInfo_Consumable[] ConsumalbeInfos;

    private List<Dictionary<string, object>> QualityProb; // 장비 등급 확률표

    public SpellInfo GetInfo_Spell(string ID)
    {
        foreach (SpellInfo Data in spellInfos)
            if (Data.ID == ID)
                return Data;

        return null;
    }

    public ItemInfo_Equipment GetInfo_Equipment(string ID)
    {
        foreach (ItemInfo_Equipment Data in EquipmentInfos)
            if (Data.ID == ID)
                return Data;

        return null;
    }

    public List<ItemInfo_Equipment> GetInfo_Equipments(int Level)
    {
        if (Level > 50) Level = 50;

        List<ItemInfo_Equipment> array = new List<ItemInfo_Equipment>();
        foreach (ItemInfo_Equipment Data in EquipmentInfos)
            if (Data.LimitLevel / 10 == Level / 10)
                array.Add(Data);

        return array;
    }

    public ItemInfo_Equipment GetInfo_Equipment(int Level)
    {
        if (Level > 50) Level = 50;

        List<ItemInfo_Equipment> array = new List<ItemInfo_Equipment>();
        foreach (ItemInfo_Equipment Data in EquipmentInfos)
            if (Data.LimitLevel / 10 == Level / 10)
                array.Add(Data);

        int RandomNum = Random.Range(0, array.Count);

        return array[RandomNum];
    }

    public ItemInfo_Consumable GetInfo_Consumable(string ID)
    {
        foreach (ItemInfo_Consumable Data in ConsumalbeInfos)
                if (Data.ID == ID)
                    return Data;

        return null;
    }

    public List<ItemInfo_Consumable> GetInfo_Consumables(int Level)
    {
        if (Level > 50) Level = 50;

        List<ItemInfo_Consumable> array = new List<ItemInfo_Consumable>();
        foreach (ItemInfo_Consumable Data in ConsumalbeInfos)
            if (Data.LimitLevel <= Level)
                array.Add(Data);

        return array;
    }

    public ItemInfo_Consumable GetInfo_Consumable(int Level)
    {
        if (Level > 50) Level = 50;

        List<ItemInfo_Consumable> array = new List<ItemInfo_Consumable>();
        foreach (ItemInfo_Consumable Data in ConsumalbeInfos)
            if (Data.LimitLevel / 10 == Level / 10)
                array.Add(Data);

        int RandomNum = Random.Range(0, array.Count);

        return array[RandomNum];
    }

    public Item_Base.Qualitys GetQuality(int Level)
    {
        if (Level > 50) Level = 50;

        float[] myQualityProb = new float[6];
        int a = 0;
        foreach (var value in QualityProb[Level / 10].Values) // 레벨마다 다른 확률을 엑셀로 가져와서 배열에 할당
            myQualityProb[a++] = (float)System.Convert.ToDouble(value);
        int newQuality = (int)ChanceMaker.Choose(myQualityProb); // 할당된 확률 배열로 가중치 랜덤뽑기로 등급 설정

        return (Item_Base.Qualitys)newQuality;
    } 

    private void Awake()
    {
        LoadDataTable_SpellInfo();
        LoadDataTable_EquipmentInfo();
        LoadDataTable_Consumable();
        QualityProb = CSVReader.Read("EquipmentQualityProb"); // 장비 등급 확률표 읽어옴
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

                case "AE":
                    info.Type = SpellInfo.SpellType.AE;
                    break;

                case "AOE":
                    info.Type = SpellInfo.SpellType.AOE;
                    break;

                case "Target":
                    info.Type = SpellInfo.SpellType.Target;
                    break;

                case "Turret":
                    info.Type = SpellInfo.SpellType.Turret;
                    break;

                case "Toggle":
                    info.Type = SpellInfo.SpellType.Toggle;
                    break;

                case "Passive":
                    info.Type = SpellInfo.SpellType.Passive;
                    break;

                case "Buff":
                    info.Type = SpellInfo.SpellType.Buff;
                    break;

                case "None":
                    info.Type = SpellInfo.SpellType.Move;
                    break;
                    #endregion
            }
            info.Prefab = Resources.Load<GameObject>("Prefabs/Skills/" + DataTable_Spell[i]["Prefab"].ToString());
            info.Icon = Resources.Load<Sprite>("Sprites/" + DataTable_Spell[i]["Icon"].ToString());
            info.Name = DataTable_Spell[i]["Name"].ToString();
            info.Description = DataTable_Spell[i]["Description"].ToString();
            info.CoolTime = float.Parse(DataTable_Spell[i]["CoolTime"].ToString());
            info.ManaCost = int.Parse(DataTable_Spell[i]["ManaCost"].ToString());
            info.Speed = int.Parse(DataTable_Spell[i]["Speed"].ToString());
            info.SpellxDamage = float.Parse(DataTable_Spell[i]["SpellxDamage"].ToString());
            info.Sound = DataTable_Spell[i]["Sound"].ToString();

            spellInfos[i] = info;
        }
    }

    private void LoadDataTable_EquipmentInfo()
    {
        List<Dictionary<string, object>> DataTable_Equipment = CSVReader.Read("DataTable_Equipment");
        EquipmentInfos = new ItemInfo_Equipment[DataTable_Equipment.Count];

        for (int i = 0; i < DataTable_Equipment.Count; i++)
        {
            ItemInfo_Equipment info = new ItemInfo_Equipment();
            // ItemInfo_Base
            info.ID = DataTable_Equipment[i]["ID"].ToString();
            info.Kind = ItemInfo_Base.Kinds.Equipment;
            info.Icon = Resources.Load<Sprite>("Sprites/" + DataTable_Equipment[i]["Icon"].ToString());
            info.Name = DataTable_Equipment[i]["Name"].ToString();
            info.Descript = DataTable_Equipment[i]["Effect"].ToString();
            info.LimitLevel = int.Parse(DataTable_Equipment[i]["LimitLevel"].ToString());
            info.Cost = int.Parse(DataTable_Equipment[i]["Cost"].ToString());
            // ItemInfo_Equipment
            switch (DataTable_Equipment[i]["Part"])
            {
                #region 파트 구분
                case "Helmet":
                    info.Part = ItemInfo_Equipment.Parts.Helmet;
                    break;

                case "Cloth":
                    info.Part = ItemInfo_Equipment.Parts.Cloth;
                    break;

                case "Shoes":
                    info.Part = ItemInfo_Equipment.Parts.Shoes;
                    break;

                case "Weapon":
                    info.Part = ItemInfo_Equipment.Parts.Weapon;
                    break;

                case "Shoulder":
                    info.Part = ItemInfo_Equipment.Parts.Shoulder;
                    break;

                case "Back":
                    info.Part = ItemInfo_Equipment.Parts.Back;
                    break;
                    #endregion
            }
            info.ItemSprite = DataTable_Equipment[i]["ItemSprite"].ToString();
            info.BaseOption = DataTable_Equipment[i]["BaseOption"].ToString();
            info.BaseOptionValue = int.Parse(DataTable_Equipment[i]["BaseOptionValue"].ToString());

            EquipmentInfos[i] = info;
        }
    }

    private void LoadDataTable_Consumable()
    {
        List<Dictionary<string, object>> DataTable_Consumable = CSVReader.Read("DataTable_Consumable");
        ConsumalbeInfos = new ItemInfo_Consumable[DataTable_Consumable.Count];

        for (int i = 0; i < DataTable_Consumable.Count; i++)
        {
            string[] kind = DataTable_Consumable[i]["ID"].ToString().Split('_');
            switch (kind[1])
            {
                case "Potion":
                    ItemInfo_Potion info = new ItemInfo_Potion();
                    // ItemInfo_Base;
                    info.ID = DataTable_Consumable[i]["ID"].ToString();
                    info.Kind = ItemInfo_Base.Kinds.Potion;
                    info.Icon = Resources.Load<Sprite>("Sprites/" + DataTable_Consumable[i]["Icon"].ToString());
                    info.Name = DataTable_Consumable[i]["Name"].ToString();
                    info.Descript = DataTable_Consumable[i]["Effect"].ToString();
                    info.LimitLevel = int.Parse(DataTable_Consumable[i]["LimitLevel"].ToString());
                    info.Cost = int.Parse(DataTable_Consumable[i]["Cost"].ToString());
                    // ItemInfo_Consumable
                    info.StackSize = int.Parse(DataTable_Consumable[i]["StackSize"].ToString());
                    // ItemInfo_Potion
                    info.BuffName = DataTable_Consumable[i]["BuffName"].ToString();
                    info.Value = int.Parse(DataTable_Consumable[i]["Value"].ToString());

                    ConsumalbeInfos[i] = info;
                    break;
            }
        }
    }
}