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

    public BagScript MyBagScript { get; set; }

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

    // 아이템 사용
    public void Use()
    {
        // bagPrefab 아이템을 만들고 BagScript 를 참조한다.
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();

        // slot 아이템을 Bag 안에 추가한다.
        MyBagScript.AddSlots(slots);


    }
}