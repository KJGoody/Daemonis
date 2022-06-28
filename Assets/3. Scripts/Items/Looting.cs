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
            if(D_item.isKind == DropItem.IsKind.Gold) // ���� �׳� ����
            {
                D_item.L_Start = true;
            }
            else if (OptionPanel.MyInstance.lootingQuality[(int)D_item.Item.quality].isOn) // �ɼǿ��� �ش� ����� ���õ��ִ���
            {
                D_item.L_Start = true;
            }
        }
    }
}
