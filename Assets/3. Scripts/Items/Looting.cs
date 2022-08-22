using UnityEngine;

public class Looting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            ItemCart DropItem = collision.GetComponent<ItemCart>();

            if(DropItem.Kind == ItemCart.Kinds.Gold) // ���� �׳� ����
            {
                DropItem.IsLooting = true;
            }
            else if (OptionPanel.MyInstance.lootingQuality[(int)DropItem.Item.Quality].isOn) // �ɼǿ��� �ش� ����� ���õ��ִ���
            {
                DropItem.IsLooting = true;
            }
        }
    }
}
