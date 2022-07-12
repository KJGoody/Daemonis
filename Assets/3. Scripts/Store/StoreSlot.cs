using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : Slot_Base
{
    [SerializeField]
    private Text ItemName;
    [SerializeField]
    private Text ItemCost;

    [HideInInspector]
    public Item_Base Item;

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
            ItemName.text = null;
            ItemCost.gameObject.SetActive(false);
        }
    }


}
