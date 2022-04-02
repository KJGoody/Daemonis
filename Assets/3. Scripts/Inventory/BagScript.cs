using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{

    [SerializeField]
    private GameObject slotPrefab;
    // ���� ���� ���� ����Ʈ
    private List<SlotScript> slots = new List<SlotScript>();
    // ���濡 ������ �߰��Ѵ�.
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
            // �� ������ ������
            if (slot.IsEmpty)
            {
                // �ش� ���Կ� �������� �߰��Ѵ�.
                slot.AddItem(item);
                return true;
            }
        }

        return false;
    }

}
