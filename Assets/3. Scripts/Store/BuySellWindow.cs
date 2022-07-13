using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuySellWindow : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup StorePanelView;
    [SerializeField]
    private StoreSlot ItemView;
    [SerializeField]
    private Text ItemCountText;
    private int ItemCount;
    [SerializeField]
    private Text CostText;
    [SerializeField]
    private Text ChangeText;

    private int Bill;

    public void SetWindow(bool IsBuy, Item_Base item)
    {
        ItemView.SetSlot(item);
        ItemCount = 1;
        ItemCountText.text = ItemCount.ToString();
        ChangeText.color = Color.white;

        if (IsBuy)
        {
            CostText.text = (ItemView.Item.MyCost * ItemCount).ToString();
            Bill = GameManager.MyInstance.DATA.Gold - (ItemView.Item.MyCost * ItemCount);
            ChangeText.text = Bill.ToString();
        }

        gameObject.SetActive(true);
        StorePanelView.blocksRaycasts = false;
    }

    public void _CloseWindow()
    {
        gameObject.SetActive(false);
        StorePanelView.blocksRaycasts = true;
    }

    public void _ItemCountAddSub(int Num)
    {
        ItemCount += Num;
        if(ItemCount <= 0)
            ItemCount = 1;
        if (ItemCount > 100)
            ItemCount = 100;

        ItemCountText.text = ItemCount.ToString();
        CostText.text = (ItemView.Item.MyCost * ItemCount).ToString();
        Bill = GameManager.MyInstance.DATA.Gold - (ItemView.Item.MyCost * ItemCount);
        ChangeText.text = Bill.ToString();
        if (Bill < 0)
            ChangeText.color = Color.red;
        else
            ChangeText.color = Color.white;
    }

    public void _BuyItem()
    {
        if(Bill >= 0)
        {
            if(ItemView.Item is Item_Consumable)
            {

            }
            else
            {
                if(InventoryScript.MyInstance.GetEmptySlotNum() >= ItemCount)
                {

                    InventoryScript.MyInstance.AddItem(ItemView.Item);
                }
            }
        }
    }
}
