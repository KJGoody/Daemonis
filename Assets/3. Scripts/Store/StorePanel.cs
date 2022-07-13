using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    private static StorePanel instance;
    public static StorePanel Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<StorePanel>();
            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup storePanel;
    [SerializeField]
    private CanvasGroup InventoryPanel;

    [SerializeField]
    private GameObject RemoveButton;
    [SerializeField]
    private GameObject SellButton;

    [SerializeField]
    private Transform ProductView;
    [SerializeField]
    private Transform StuffTap;

    [SerializeField]
    private StoreSlot[] StoreSlots;
    private Item_Base[] StoreSlots_Stuff = new Item_Base[4];
    private Item_Equipment[] StoreSlots_Equipment = new Item_Equipment[4];

    private enum CurrentTapName { Stuff, Equipment, Sell }
    private CurrentTapName CurrentTap;

    private DataArray_Item_Equipment[] equipmentPerLv; // 기본 장비 아이템 리스트 열에 해당되는 이름
    private List<Dictionary<string, object>> qualityProb; // 장비 등급 확률표
    private DataArray_Item_Consumable[] HealthPotionLv;

    [HideInInspector]
    public bool CanReStock = true;

    public void OpenStore()
    {
        storePanel.alpha = 1;
        storePanel.blocksRaycasts = true;

        if(InventoryPanel.alpha != 1)
        {
            UIManager.MyInstance.OpenClose(InventoryPanel);
        }

        RemoveButton.SetActive(false);
        SellButton.SetActive(true);
    }

    public void _CloseStore()
    {
        _SelectTap(StuffTap);
        BuySellWindow.Instance._CloseWindow();

        RemoveButton.SetActive(true);
        SellButton.SetActive(false);
    }

    private void Start()
    {
        equipmentPerLv = DataTableManager.Instance.GetDataTable_Item_Equipment.Data_Item_Equipments;
        qualityProb = CSVReader.Read("EquipmentQualityProb"); // 장비 등급 확률표 읽어옴
        HealthPotionLv = DataTableManager.Instance.GetDataTable_Item_Consumable.Data_Item_Consumables;

        ReStock();
        _SelectTap(StuffTap);
    }

    private void ReStock()
    {
        if (CanReStock)
        {
            SetStockItem_Stuff();
            SetStockItem_Equipment();
            CanReStock = false;
        }
    }

    private void SetStockItem_Stuff()
    {
        for(int i = 0; i < 4; i++)
        {
            StoreSlots_Stuff[i] = new Item_Consumable();
            (StoreSlots_Stuff[i] as Item_Consumable).itemInfo = HealthPotionLv[0].items[0];
            StoreSlots_Stuff[i].quality = Item_Base.Quality.Normal;
        }
    }

    private void SetStockItem_Equipment()
    {
        for(int i = 0; i < 4; i++)
        {
            int setKind = Random.Range(0, 6);
            float[] myQualityProb = new float[6];
            int a = 0;
            foreach (var value in qualityProb[SetLvNum()].Values) // 레벨마다 다른 확률을 엑셀로 가져와서 배열에 할당
                myQualityProb[a++] = (float)System.Convert.ToDouble(value);
            int newQuality = (int)ChanceMaker.Choose(myQualityProb); // 할당된 확률 배열로 가중치 랜덤뽑기로 등급 설정

            StoreSlots_Equipment[i] = new Item_Equipment();
            StoreSlots_Equipment[i].itemInfo = equipmentPerLv[SetLvNum()].items[setKind];
            StoreSlots_Equipment[i].quality = (Item_Base.Quality)newQuality;
        }
    }

    private int SetLvNum()
    {
        if (Player.MyInstance.MyStat.Level > 50)
            Player.MyInstance.MyStat.Level = 50;
        int levelNum = Player.MyInstance.MyStat.Level / 10;
        return levelNum;
    }

    public void _SelectTap(Transform MyTransform)
    {
        MyTransform.SetSiblingIndex(2);
        ProductView.SetSiblingIndex(1);
        switch (MyTransform.name)
        {
            case "StuffTap":
                CurrentTap = CurrentTapName.Stuff;
                break;

            case "EquipmentTap":
                CurrentTap = CurrentTapName.Equipment;
                break;

            case "SellTap":
                CurrentTap = CurrentTapName.Sell;
                break;
        }

        UpdateStoreSlots();
    }

    private void UpdateStoreSlots()
    {
        switch (CurrentTap)
        {
            case CurrentTapName.Stuff:
                SetSlots(StoreSlots_Stuff);
                break;

            case CurrentTapName.Equipment:
                SetSlots(StoreSlots_Equipment);
                break;

            case CurrentTapName.Sell:
                SetSlots(null);
                break;
        }
    }

    private void SetSlots(Item_Base[] storeSlots)
    {
        for (int i = 0; i < 4; i++)
        {
            if (storeSlots != null)
                StoreSlots[i].SetSlot(storeSlots[i]);
            else
                StoreSlots[i].SetSlot(null);
        }
    }
}
