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
    public void AddItem(Item item)
    {
 
        foreach(Bag bag in bags)
        {
            // 가방 리스트 중에 빈슬롯 이 있는
            // 가방을 찾고 해당 가방에 아이템을 추가합니다.
            if(bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
        // 빈 슬롯이 아예 없는 경우에 대한 예외처리가 아직 안되었네요.
    }


}
