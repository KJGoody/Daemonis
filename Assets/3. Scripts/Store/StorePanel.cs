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

    [SerializeField] private CanvasGroup storePanel;
    [SerializeField] private CanvasGroup InventoryPanel;

    [SerializeField] private GameObject Joystick;

    [SerializeField] private GameObject RemoveButton;
    [SerializeField] private GameObject SellButton;

    [SerializeField] private Transform ProductView;
    [SerializeField] private Transform StuffTap;

    [SerializeField]
    private StoreSlot[] StoreSlots;
    private Item_Consumable[] StoreSlots_Stuff = new Item_Consumable[4];
    private Item_Equipment[] StoreSlots_Equipment = new Item_Equipment[4];

    private enum CurrentTapName { Stuff, Equipment, Sell }
    private CurrentTapName CurrentTap;

    [HideInInspector] public bool CanReStock = true;

    public void OpenStore()
    {
        storePanel.alpha = 1;
        storePanel.blocksRaycasts = true;

        if (InventoryPanel.alpha != 1)
        {
            UIManager.MyInstance.OpenClose(InventoryPanel);
        }

        Joystick.SetActive(false);
        RemoveButton.SetActive(false);
        SellButton.SetActive(true);
    }

    public void _CloseStore()
    {
        _SelectTap(StuffTap);
        BuySellWindow.Instance._CloseWindow();

        Joystick.SetActive(true);
        RemoveButton.SetActive(true);
        SellButton.SetActive(false);
    }

    private void Start()
    {
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
        for (int i = 0; i < 4; i++)
            StoreSlots_Stuff[i] = new Item_Consumable();

        for (int i = 0; i < 4; i++)
        {
            ItemInfo_Consumable tempInfo = new ItemInfo_Consumable();
            List<ItemInfo_Consumable> Array = DataTableManager.Instance.GetItemInfo_Consumables(Player.MyInstance.MyStat.Level);
            int TryCount = 0;
            do
            {
                if (TryCount >= Array.Count)
                {
                    TryCount++;
                    break;
                }
                else
                    tempInfo = Array[TryCount++];

            } while (IsAlrealyStock(tempInfo));

            if (TryCount > Array.Count)
                break;

            string[] kind = tempInfo.ID.Split('_');
            switch (kind[1])
            {
                case "Potion":
                    StoreSlots_Stuff[i] = new Item_Potion();
                    (StoreSlots_Stuff[i] as Item_Potion).SetInfo(tempInfo as ItemInfo_Potion);
                    break;
            }
            StoreSlots_Stuff[i].Quality = Item_Base.Qualitys.Normal;
        }
    }

    private bool IsAlrealyStock(ItemInfo_Consumable ItemInfo)
    {
        for (int i = 0; i < 4; i++)
        {
            if (StoreSlots_Stuff[i].IsSetInfo)
                if (StoreSlots_Stuff[i].ID == ItemInfo.ID)
                    return true;
        }

        return false;
    }

    private void SetStockItem_Equipment()
    {
        for (int i = 0; i < 4; i++)
        {
            StoreSlots_Equipment[i] = new Item_Equipment();
            StoreSlots_Equipment[i].SetInfo(DataTableManager.Instance.GetItemInfo_Equipment(Player.MyInstance.MyStat.Level));
            StoreSlots_Equipment[i].Quality = DataTableManager.Instance.GetQuality(Player.MyInstance.MyStat.Level);
        }
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
