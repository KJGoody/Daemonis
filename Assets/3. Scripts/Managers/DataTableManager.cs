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

    //-- StageInfo --
    private StageInfo[] StageInfos;
    private void LoadDataTable_Stage()
    {
        List<Dictionary<string, object>> DataTable_Stage = CSVReader.Read("DataTable_Stage");
        StageInfos = new StageInfo[DataTable_Stage.Count];

        for (int i = 0; i < DataTable_Stage.Count; i++)
        {
            StageInfo info = new StageInfo();
            info.ID = DataTable_Stage[i]["ID"].ToString();
            info.MapPrefabs = DataTable_Stage[i]["MapPrefabs"].ToString();
            info.EnemyMaxNum = int.Parse(DataTable_Stage[i]["EnemyMaxNum"].ToString());
            info.EnemyMinNum = int.Parse(DataTable_Stage[i]["EnemyMinNum"].ToString());
            info.ElitePercent = int.Parse(DataTable_Stage[i]["ElitePercent"].ToString());
            info.GuvPercent = int.Parse(DataTable_Stage[i]["GuvPercent"].ToString());
            info.InvadeGage = int.Parse(DataTable_Stage[i]["InvadeGage"].ToString());
            info.EnemyStatPercent = float.Parse(DataTable_Stage[i]["EnemyStatPercent"].ToString());

            EnemyTypeInfo bossinfo = new EnemyTypeInfo();
            bossinfo.ID = DataTable_Stage[i]["BossID"].ToString();
            bossinfo.Name = DataTable_Stage[i]["Name"].ToString();
            bossinfo.Prefab = Resources.Load<GameObject>("Prefabs/Enemy/" + DataTable_Stage[i]["Prefab"].ToString());
            bossinfo.Sound = DataTable_Stage[i]["Sound"].ToString();
            switch (DataTable_Stage[i]["AttackType"].ToString())
            {
                case "BaseMelee":
                    bossinfo.AttackType = EnemyTypeInfo.AttackTypes.BaseMelee;
                    break;

                case "Kobold_Ranged":
                    bossinfo.AttackType = EnemyTypeInfo.AttackTypes.Kobold_Ranged;
                    break;
            }
            bossinfo.AttackRange = float.Parse(DataTable_Stage[i]["AttackRange"].ToString());
            bossinfo.AttackDelay = float.Parse(DataTable_Stage[i]["AttackDelay"].ToString());
            bossinfo.EXP = int.Parse(DataTable_Stage[i]["EXP"].ToString());
            //--Stat--
            bossinfo.Level = int.Parse(DataTable_Stage[i]["Level"].ToString());
            bossinfo.Attack = int.Parse(DataTable_Stage[i]["Attack"].ToString());
            bossinfo.MaxHealth = int.Parse(DataTable_Stage[i]["MaxHealth"].ToString());
            bossinfo.MoveSpeed = int.Parse(DataTable_Stage[i]["MoveSpeed"].ToString());
            bossinfo.HitPercent = int.Parse(DataTable_Stage[i]["HitPercent"].ToString());
            info.BossInfo = bossinfo;
            info.DropTime = int.Parse(DataTable_Stage[i]["DropTime"].ToString());

            StageInfos[i] = info;
        }
    }
    public StageInfo GetStageInfo(string StageID)
    {
        foreach (StageInfo Data in StageInfos)
            if (Data.ID == StageID)
                return Data;

        return null;
    }
    //-- StageInfo --

    //-- QuestDialog --
    private DialogData[] QuestDialog;
    private void LoadDataTable_QuestDialog()
    {
        List<Dictionary<string, object>> DataTable_QuestDialog = CSVReader.Read("DataTable_QuestDialog");
        QuestDialog = new DialogData[DataTable_QuestDialog.Count];

        for (int i = 0; i < DataTable_QuestDialog.Count; i++)
        {
            DialogData info = new DialogData();
            info.QuestIndex = int.Parse(DataTable_QuestDialog[i]["QuestIndex"].ToString());
            info.QuestStat = int.Parse(DataTable_QuestDialog[i]["QuestStat"].ToString());
            info.ActorName = DataTable_QuestDialog[i]["ActorName"].ToString();
            info.Speech = DataTable_QuestDialog[i]["Speech"].ToString();

            QuestDialog[i] = info;
        }
    }
    public List<DialogData> GetDialogArray()
    {
        int questIndex = GameManager.MyInstance.DATA.CurrentQuestIndex;

        List<DialogData> array = new List<DialogData>();
        for (int i = 0; i < QuestDialog.Length; i++)
            if(questIndex == QuestDialog[i].QuestIndex)
                if (GameManager.MyInstance.DATA.QuestStat[questIndex] == QuestDialog[i].QuestStat)
                    array.Add(QuestDialog[i]);

        return array;
    }
    //-- QuestDialog --

    //-- SpellInfo --
    private SpellInfo[] spellInfos;
    public SpellInfo[] SpellInfos { get { return spellInfos; } }
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
    public SpellInfo GetInfo_Spell(string ID)
    {
        foreach (SpellInfo Data in spellInfos)
            if (Data.ID == ID)
                return Data;

        return null;
    }
    //-- SpellInfo --

    //-- EquipmentInfo --
    private ItemInfo_Equipment[] EquipmentInfos;
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
    //-- EquipmentInfo --

    //-- ConsumableInfo --
    private ItemInfo_Consumable[] ConsumableInfos;
    private void LoadDataTable_Consumable()
    {
        List<Dictionary<string, object>> DataTable_Consumable = CSVReader.Read("DataTable_Consumable");
        ConsumableInfos = new ItemInfo_Consumable[DataTable_Consumable.Count];

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

                    ConsumableInfos[i] = info;
                    break;
            }
        }
    }
    public ItemInfo_Consumable GetInfo_Consumable(string ID)
    {
        foreach (ItemInfo_Consumable Data in ConsumableInfos)
                if (Data.ID == ID)
                    return Data;

        return null;
    }
    public List<ItemInfo_Consumable> GetInfo_Consumables(int Level)
    {
        if (Level > 50) Level = 50;

        List<ItemInfo_Consumable> array = new List<ItemInfo_Consumable>();
        foreach (ItemInfo_Consumable Data in ConsumableInfos)
            if (Data.LimitLevel <= Level)
                array.Add(Data);

        return array;
    }
    public ItemInfo_Consumable GetInfo_Consumable(int Level)
    {
        if (Level > 50) Level = 50;

        List<ItemInfo_Consumable> array = new List<ItemInfo_Consumable>();
        foreach (ItemInfo_Consumable Data in ConsumableInfos)
            if (Data.LimitLevel / 10 <= Level / 10)
                array.Add(Data);

        int RandomNum = Random.Range(0, array.Count);
        return array[RandomNum];
    }
    //-- ConsumableInfo --

    //-- EnemyInfo --
    private EnemyTypeInfo[] EnemyInfos;
    private void LoadDataTable_Enemy()
    {
        List<Dictionary<string, object>> DataTable_Enemy = CSVReader.Read("DataTable_Enemy");
        EnemyInfos = new EnemyTypeInfo[DataTable_Enemy.Count];

        for (int i = 0; i < DataTable_Enemy.Count; i++)
        {
            EnemyTypeInfo info = new EnemyTypeInfo();
            info.ID = DataTable_Enemy[i]["ID"].ToString();
            info.Name = DataTable_Enemy[i]["Name"].ToString();
            info.Prefab = Resources.Load<GameObject>("Prefabs/Enemy/" + DataTable_Enemy[i]["Prefab"].ToString());
            info.Sound = DataTable_Enemy[i]["Sound"].ToString();
            switch (DataTable_Enemy[i]["AttackType"].ToString())
            {
                case "BaseMelee":
                    info.AttackType = EnemyTypeInfo.AttackTypes.BaseMelee;
                    break;

                case "Kobold_Ranged":
                    info.AttackType = EnemyTypeInfo.AttackTypes.Kobold_Ranged;
                    break;
            }
            info.AttackRange = float.Parse(DataTable_Enemy[i]["AttackRange"].ToString());
            info.AttackDelay = float.Parse(DataTable_Enemy[i]["AttackDelay"].ToString());
            info.EXP = int.Parse(DataTable_Enemy[i]["EXP"].ToString());
            //--Stat--
            info.Level = int.Parse(DataTable_Enemy[i]["Level"].ToString());
            info.Attack = int.Parse(DataTable_Enemy[i]["Attack"].ToString());
            info.MaxHealth = int.Parse(DataTable_Enemy[i]["MaxHealth"].ToString());
            info.MoveSpeed = int.Parse(DataTable_Enemy[i]["MoveSpeed"].ToString());
            info.HitPercent = int.Parse(DataTable_Enemy[i]["HitPercent"].ToString());

            EnemyInfos[i] = info;
        }
    }
    public EnemyTypeInfo GetEnemyType(string strEnemyType)
    {
        foreach (EnemyTypeInfo Data in EnemyInfos)
            if (Data.ID == strEnemyType)
                return Data;

        return null;
    }
    public List<GameObject> GetEnemyPrefabs(string CurrentStage)
    {
        List<GameObject> array = new List<GameObject>();
        foreach (EnemyTypeInfo Data in EnemyInfos)
        {
            string[] DataSplit = Data.ID.Split('_');
            string[] CurrentStageSplit = CurrentStage.Split('_');
            if (DataSplit[1] == CurrentStageSplit[1])
                array.Add(Data.Prefab);
        }

        return array;
    }
    //-- EnemyInfo --

    //-- QualityProb --
    private List<Dictionary<string, object>> QualityProb; // 장비 등급 확률표
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
    //-- QualityProb --

    private void Awake()
    {
        LoadDataTable_Stage();
        LoadDataTable_QuestDialog();
        LoadDataTable_SpellInfo();
        LoadDataTable_EquipmentInfo();
        LoadDataTable_Consumable();

        LoadDataTable_Enemy();
        QualityProb = CSVReader.Read("EquipmentQualityProb"); // 장비 등급 확률표 읽어옴
    }

}