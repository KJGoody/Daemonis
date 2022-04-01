using UnityEngine;

public class InventoryScript : MonoBehaviour
{

    private static InventoryScript instance;
    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }

        set
        {

            instance = value;
        }
    }

    // �׽�Ʈ�� ���� �뵵
    [SerializeField]
    private Item[] items;

    private void Awake()
    {
        // ������ �����ϰ�
        Bag bag = (Bag)Instantiate(items[0]);

        // ������ ���� ������ �����ϰ�
        bag.Initalize(16);

        // ���� �������� ����Ѵ�.
        bag.Use();
    }
    public void AddItem(Item item)
    {
 
        foreach(Bag bag in bags)
        {
            // ���� ����Ʈ �߿� �󽽷� �� �ִ�
            // ������ ã�� �ش� ���濡 �������� �߰��մϴ�.
            if(bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
        // �� ������ �ƿ� ���� ��쿡 ���� ����ó���� ���� �ȵǾ��׿�.
    }


}
