using UnityEngine;

public class BagScript : MonoBehaviour
{

    [SerializeField]
    private GameObject slotPrefab;

    // 가방에 슬롯을 추가한다.
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, transform);
        }
    }
}
