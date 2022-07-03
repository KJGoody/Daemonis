using UnityEngine;

public class Looting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            ItemCart DropItem = collision.GetComponent<ItemCart>();

            if(DropItem.isKind == ItemCart.IsKind.Gold) // 골드면 그냥 루팅
            {
                DropItem.IsLooting = true;
            }
            else if (OptionPanel.MyInstance.lootingQuality[(int)DropItem.Item.quality].isOn) // 옵션에서 해당 등급이 선택돼있는지
            {
                DropItem.IsLooting = true;
            }
        }
    }
}
