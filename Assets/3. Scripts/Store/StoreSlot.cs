using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : Slot_Base
{
    [SerializeField] protected Text ItemName;
    public Text ItemCost;

    [HideInInspector] public Item_Base Item;

    private bool CnaBuy = true;

    protected virtual void Update()
    {
        if (Item != null)
        {
            if (Item.Cost > GameManager.MyInstance.DATA.Gold)
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
        if (item != null)
        {
            Item = item;
            icon.sprite = Item.Icon;
            ItemName.text = Item.Name;
            ItemCost.text = Item.Cost.ToString();
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
        if (Item != null && CnaBuy)
        {
            BuySellWindow.Instance.SetWindow(true, Item);
        }
    }
}
