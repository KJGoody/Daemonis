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
    private ItemInfo_Consumable[] consumalbeInfos;

    private List<Dictionary<string, object>> QualityProb; // ��� ��� Ȯ��ǥ

    public SpellInfo GetSpellData(string ID)
    {
        foreach (SpellInfo Data in spellInfos)
            if (Data.ID == ID)
                return Data;

        return null;
    }

    public ItemInfo_Equipment GetItemInfo_Equipment(string ID)
    {
        foreach (ItemInfo_Equipment Data in equipmentInfos)
            if (Data.ID == ID)
                return Data;

        return null;
    }

    public ItemInfo_Equipment GetItemInfo_Equipment(int Level)
    {
        if (Level > 50) Level = 50;

        List<ItemInfo_Equipment> array = new List<ItemInfo_Equipment>();
        foreach (ItemInfo_Equipment Data in equipmentInfos)
            if (Data.LimitLevel / 10 == Level / 10)
                array.Add(Data);

        int RandomNum = Random.Range(0, array.Count);

        return array[RandomNum];
    }

    public ItemInfo_Consumable GetItemInfo_Consumable(string ID)
    {
        foreach (ItemInfo_Consumable Data in consumalbeInfos)
                if (Data.ID == ID)
                    return Data;

        return null;
    }

    public ItemInfo_Consumable GetItemInfo_Consumable(int Level)
    {
        if (Level > 50) Level = 50;

        List<ItemInfo_Consumable> array = new List<ItemInfo_Consumable>();
        foreach (ItemInfo_Consumable Data in consumalbeInfos)
            if (Data.LimitLevel <= Level)
                array.Add(Data);

        int RandomNum = Random.Range(0, array.Count);

        return array[RandomNum];
    }

    public Item_Base.Quality GetQuality(int Level)
    {
        if (Level > 50) Level = 50;

        float[] myQualityProb = new float[6];
        int a = 0;
        foreach (var value in QualityProb[Level / 10].Values) // �������� �ٸ� Ȯ���� ������ �����ͼ� �迭�� �Ҵ�
            myQualityProb[a++] = (float)System.Convert.ToDouble(value);
        int newQuality = (int)ChanceMaker.Choose(myQualityProb); // �Ҵ�� Ȯ�� �迭�� ����ġ �����̱�� ��� ����

        return (Item_Base.Quality)newQuality;
    } 

    private void Awake()
    {
        LoadDataTable_SpellInfo();
        LoadDataTable_EquipmentInfo();
        LoadDataTable_Consumable();
        QualityProb = CSVReader.Read("EquipmentQualityProb"); // ��� ��� Ȯ��ǥ �о��
    }

    private void LoadDataTable_SpellInfo()
    {
        // ���� ������ ���̺��� �ҷ�����
        List<Dictionary<string, object>> DataTable_Spell = CSVReader.Read("DataTable_Spell");
        // ������ ���̺��� ũ��� 
        spellInfos = new SpellInfo[DataTable_Spell.Count];

        for (int i = 0; i < DataTable_Spell.Count; i++)
        {
            // SpellInfo�� �����Ͽ� �����͸� �Է��Ѵ�.
            SpellInfo info = new SpellInfo();
            info.ID = DataTable_Spell[i]["ID"].ToString();
            switch (DataTable_Spell[i]["Type"].ToString())
            {
                #region ���� Ÿ��
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
        List<Dictionary<string, object>> DataTable_Equipment = CSVReader.Read("DataTable_Equipment");
        equipmentInfos = new ItemInfo_Equipment[DataTable_Equipment.Count];

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
                #region ��Ʈ ����
                case "Helmet":
                    info.part = ItemInfo_Equipment.Part.Helmet;
                    break;

                case "Cloth":
                    info.part = ItemInfo_Equipment.Part.Cloth;
                    break;

                case "Shoes":
                    info.part = ItemInfo_Equipment.Part.Shoes;
                    break;

                case "Weapon":
                    info.part = ItemInfo_Equipment.Part.Weapon;
                    break;

                case "Shoulder":
                    info.part = ItemInfo_Equipment.Part.Shoulder;
                    break;

                case "Back":
                    info.part = ItemInfo_Equipment.Part.Back;
                    break;
                    #endregion
            }
            info.ItemSprite = DataTable_Equipment[i]["ItemSprite"].ToString();
            info.BaseOption = DataTable_Equipment[i]["BaseOption"].ToString();
            info.BaseOptionValue = int.Parse(DataTable_Equipment[i]["BaseOptionValue"].ToString());

            equipmentInfos[i] = info;
        }
    }

    private void LoadDataTable_Consumable()
    {
        List<Dictionary<string, object>> DataTable_Consumable = CSVReader.Read("DataTable_Consumable");
        consumalbeInfos = new ItemInfo_Consumable[DataTable_Consumable.Count];

        for (int i = 0; i < DataTable_Consumable.Count; i++)
        {
            string[] kind = DataTable_Consumable[i]["ID"].ToString().Split('_');
            switch (kind[1])
            {
                case "Potion":
                    ItemInfo_Potion info = new ItemInfo_Potion();
                    // ItemInfo_Base;
                    info.ID = DataTable_Consumable[i]["ID"].ToString();
                    info.Kind = ItemInfo_Base.Kinds.Equipment;
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

                    consumalbeInfos[i] = info;
                    break;
            }
        }
    }
}