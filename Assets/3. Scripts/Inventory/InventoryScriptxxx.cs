using System.Collections.Generic;
using UnityEngine;

public class InventoryScriptxxx : MonoBehaviour
{
    //// 테스트를 위한 용도
    ////[SerializeField]
    ////private Item[] items;
    //private List<Bag> bags = new List<Bag>();
    //public bool CanAddBag
    //{
    //    get { return bags.Count < 5; }
    //}
    //private void Update()
    //{
    //    // 테스트 용도
    //    // J 키를 누르면 가방이 BagButton에 추가됨.
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        //Bag bag = (Bag)Instantiate(items[0]);
    //        //bag.Initalize(20);
    //        //bag.Use();
    //    }
    //}
    ////public void OpenClose()
    ////{
    ////    // 모든 가방이 닫혔는지 확인.
    ////    bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);

    ////    foreach (Bag bag in bags)
    ////    {
    ////        // 모든 가방을 닫거나, 엽니다.
    //        if (bag.MyBagScript.IsOpen != closedBag)
    //        {
    //            bag.MyBagScript.OpenClose();
    //        }
    //    }
    //}

    //public void AddBag(Bag bag)
    //{
    //    // 빈 가방을 찾아서 등록한다.
    //    foreach (BagButton bagbutton in bagButtons)
    //    {
    //        if (bagbutton.MyBag == null)
    //        {
    //            bagbutton.MyBag = bag;
    //            bags.Add(bag);
    //            break;
    //        }
    //    }
    //}

    //public void AddItem(Item item)
    //{
 
    //    foreach(Bag bag in bags)
    //    {
    //        // 가방 리스트 중에 빈슬롯 이 있는
    //        // 가방을 찾고 해당 가방에 아이템을 추가합니다.
    //        if(bag.MyBagScript.AddItem(item))
    //        {
    //            return;
    //        }
    //    }
    //    // 빈 슬롯이 아예 없는 경우에 대한 예외처리가 아직 안되었네요.
    //}


}
