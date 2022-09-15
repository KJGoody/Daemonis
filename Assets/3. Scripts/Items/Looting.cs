using UnityEngine;

public class Looting : MonoBehaviour
{
    private static Looting instance;
    public static Looting Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Looting>();
            return instance;
        }
    }

    [HideInInspector] public int LootingSlotNum;
    [HideInInspector] public int LootingCansStackNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            ItemCart DropItem = collision.GetComponent<ItemCart>();

            if(DropItem.Kind == ItemCart.Kinds.Gold) // 골드면 그냥 루팅
            {
                DropItem.IsLooting = true;
            }
            else if (OptionPanel.MyInstance.lootingQuality[(int)DropItem.Item.Quality].isOn) // 옵션에서 해당 등급이 선택돼있는지
            {
                switch (DropItem.Item.Kind)
                {
                    case ItemInfo_Base.Kinds.Equipment:
                        if (InventoryScript.MyInstance.GetEmptySlotNum() - LootingSlotNum > 0)
                        {
                            DropItem.IsLooting = true;
                            LootingSlotNum++;
                        }
                        break;

                    case ItemInfo_Base.Kinds.Potion:
                        if (InventoryScript.MyInstance.CanStackNum(DropItem.Item) - LootingSlotNum > 0)
                        {
                            DropItem.IsLooting = true;
                            LootingCansStackNum++;
                        }
                        break;

                }
            }
        }
    }
}
