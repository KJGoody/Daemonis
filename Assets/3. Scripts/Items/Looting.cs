using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            DropItem D_item = collision.GetComponent<DropItem>();
            if(D_item.isKind == DropItem.IsKind.Gold) // 골드면 그냥 루팅
            {
                D_item.L_Start = true;
            }
            else if (OptionPanel.MyInstance.lootingQuality[(int)D_item.Item.quality].isOn) // 옵션에서 해당 등급이 선택돼있는지
            {
                D_item.L_Start = true;
            }
        }
    }
}
