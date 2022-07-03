using UnityEngine;

public class Looting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            ItemCart DropItem = collision.GetComponent<ItemCart>();

            if(DropItem.isKind == ItemCart.IsKind.Gold) // ���� �׳� ����
            {
                DropItem.IsLooting = true;
            }
            else if (OptionPanel.MyInstance.lootingQuality[(int)DropItem.Item.quality].isOn) // �ɼǿ��� �ش� ����� ���õ��ִ���
            {
                DropItem.IsLooting = true;
            }
        }
    }
}
