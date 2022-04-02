using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{

    [SerializeField]
    private GameObject slotPrefab;
    // 가방 안의 슬롯 리스트
    private List<SlotScript> slots = new List<SlotScript>();
    // 가방에 슬롯을 추가한다.
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slots.Add(slot);
        }
    }
    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in slots)
        {
            // 빈 슬롯이 있으면
            if (slot.IsEmpty)
            {
                // 해당 슬롯에 아이템을 추가한다.
                slot.AddItem(item);
                return true;
            }
        }

        return false;
    }

}
