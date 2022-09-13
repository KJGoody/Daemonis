using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuySellWindow : MonoBehaviour
{
    private static BuySellWindow instance;
    public static BuySellWindow Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BuySellWindow>();
            return instance;
        }
    }

    [SerializeField] private Text BuySellText;
    [SerializeField] private StoreSlot ItemView;
    [SerializeField] private GameObject ItemCountButtons;
    [SerializeField] private Text ItemCountText;
    private int ItemCount;

    [SerializeField] private Text CostMent;
    [SerializeField] private Text CostText;
    private int Cost;

    [SerializeField] private Text ChangeMent;
    [SerializeField] private Text ChangeText;

    [SerializeField] private Text BuySellButtonText;

    private bool WindowState_IsBuy;

    private int Bill;

    private delegate void Click();
    private event Click ClickButton;

    public void SetWindow(bool IsBuy, Item_Base item)
    {
        WindowState_IsBuy = IsBuy;
        ItemView.SetSlot(item);
        ItemCount = 1;
        ItemCountText.text = ItemCount.ToString();
        ChangeText.color = Color.white;

        ItemCountButtons.SetActive(true);

        if (WindowState_IsBuy)
        {
            BuySellText.text = "구매";
            BuySellButtonText.text = "구매";
            CostMent.text = "구매 금액";
            ChangeMent.text = "구매 후 금액";

            Cost = ItemView.Item.Cost;
            Bill = GameManager.MyInstance.DATA.Gold - (Cost * ItemCount);
            ClickButton = BuyItem;
        }
        else
        {
            if(item is Item_Equipment)
                ItemCountButtons.SetActive(false);

            BuySellText.text = "판매";
            BuySellButtonText.text = "판매";
            CostMent.text = "판매 금액";
            ChangeMent.text = "판매 후 금액";

            Cost = ItemView.Item.Cost / 2;
            ItemView.ItemCost.text = Cost.ToString();
            Bill = GameManager.MyInstance.DATA.Gold + (Cost * ItemCount);
            ClickButton = SellItem;
        }

        CostText.text = (Cost * ItemCount).ToString();
        ChangeText.text = Bill.ToString();

        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void _CloseWindow()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void _ItemCountAddSub(int Num)
    {
        ItemCount += Num;

        if (WindowState_IsBuy)
        {
            if (ItemCount <= 0)
                ItemCount = 1;
            if (ItemCount > 100)
                ItemCount = 100;

            Bill = GameManager.MyInstance.DATA.Gold - (Cost * ItemCount);

        }
        else
        {
            int MaxItemCount = 0;
            foreach (SlotScript slot in GameManager.MyInstance.Slots)
                if (!slot.IsEmpty)
                    if (slot.MyItem.Name == ItemView.Item.Name)
                        MaxItemCount += slot.MyCount;

            if (ItemCount <= 0)
                ItemCount = 1;
            if (ItemCount > MaxItemCount)
                ItemCount = MaxItemCount;

            Bill = GameManager.MyInstance.DATA.Gold + (Cost * ItemCount);
        }

        ItemCountText.text = ItemCount.ToString();
        CostText.text = (Cost * ItemCount).ToString();
        ChangeText.text = Bill.ToString();
        if (Bill < 0)
            ChangeText.color = Color.red;
        else
            ChangeText.color = Color.white;
    }

    public void _ClickButton()
    {
        ClickButton();
    }

    private void BuyItem()
    {
        if (Bill >= 0)
        {
            if (ItemView.Item is Item_Consumable)
            {
                if (InventoryScript.MyInstance.CanStackNum(ItemView.Item) >= ItemCount)
                {
                    _CloseWindow();
                    GameManager.MyInstance.DATA.Gold = Bill;
                    for (int i = 0; i < ItemCount; i++)
                        InventoryScript.MyInstance.AddItem(ItemView.Item, true);
                    //GameManager.MyInstance.SaveData();
                }
            }
            else
            {
                if (InventoryScript.MyInstance.GetEmptySlotNum() >= ItemCount)
                {
                    _CloseWindow();
                    GameManager.MyInstance.DATA.Gold = Bill;
                    for (int i = 0; i < ItemCount; i++)
                    {
                        Item_Equipment BuyItem = new Item_Equipment();
                        BuyItem.SetInfo((ItemView.Item as Item_Equipment).GetInfo());
                        BuyItem.Quality = ItemView.Item.Quality;
                        BuyItem.SetAddOption();
                        InventoryScript.MyInstance.AddItem(BuyItem);
                    }
                    //GameManager.MyInstance.SaveData();
                }
            }
        }
    }

    private void SellItem()
    {
        _CloseWindow();
        GameManager.MyInstance.DATA.Gold = Bill;
        for(int i = 0; i < ItemCount; i++)
            ItemView.Item.Remove();
        //GameManager.MyInstance.SaveData();
    }
}
