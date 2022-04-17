using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 상단 메뉴에 명령버튼 추가.
[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUseable
{
    private int slots;

    [SerializeField]
    protected GameObject bagPrefab;

    public InventoryScript MyBagScript { get; set; }

    // 슬롯 갯수
    public int Slots
    {
        get
        {
            return slots;
        }
    }

    public void Initalize(int slots)
    {
        // Bag의 슬롯갯수 설정
        this.slots = slots;
    }
    public string GetName()
    {
        return MyName;
    }

    //// 아이템 사용
    public void Use()
    {
        //    if (InventoryScript.MyInstance.CanAddBag)
        //    {
        //        // bagPrefab 아이템을 만들고 BagScript 를 참조한다.
        //        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();

        //        // slot 아이템을 Bag 안에 추가한다.
        //        MyBagScript.AddSlots(slots);


        //        // 인벤토리에 가방을 추가한다.
        //        // this 는 자기 자신으로 여기서는 Bag.cs 를 말한다.
        //        //InventoryScript.MyInstance.AddBag(this);
        //   }
        //InventoryScript.MyInstance.AddBag(this);
    }
}