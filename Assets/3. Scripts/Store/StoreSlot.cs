using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : Slot_Base
{
    [SerializeField]
    private Text ItemName;
    public Text ItemCost;

    [HideInInspector]
    public Item_Base Item;

    private bool CnaBuy = true;

    private void Update()
    {
        if(Item != null)
        {
            if(Item.MyCost > GameManager.MyInstance.DATA.Gold)
            {
                CnaBuy = false;
                ItemCost.color = Color.red;
            }
            else
            {
                CnaBuy = true;
                ItemCost.color = Color.white;
            }
        }
    }

    public void SetSlot(Item_Base item)
    {
        if(item != null)
        {
            Item = item;
            icon.sprite = Item.MyIcon;
            ItemName.text = Item.MyName;
            ItemCost.text = Item.MyCost.ToString();
            ItemCost.gameObject.SetActive(true);
        }
        else
        {
            Item = null;
            icon.sprite = null;
            ItemName.text = "0";
            ItemCost.gameObject.SetActive(false);
        }
    }

    public void _Click()
    {
        if(Item != null && CnaBuy)
        {
            BuySellWindow.Instance.SetWindow(true, Item);
        }
    }
}
