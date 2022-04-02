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

    // 테스트를 위한 용도
    [SerializeField]
    private Item[] items;

    private void Awake()
    {
        // 가방을 생성하고
        Bag bag = (Bag)Instantiate(items[0]);

        // 가방의 슬롯 갯수를 정의하고
        bag.Initalize(16);

        // 가방 아이템을 사용한다.
        bag.Use();
    }


}
