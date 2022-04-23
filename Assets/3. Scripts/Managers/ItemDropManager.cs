using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    private static ItemDropManager instance;

    public static ItemDropManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemDropManager>();
            }
            return instance;
        }
    }

    public DropItem dropItem;
    public Item itemInfo;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void DropItem(Transform dropPosition)
    {
        if (ChanceMaker.GetThisChanceResult_Percentage(5))
        {
            DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
            item.SetDropItem(itemInfo, Quality.Rare);
        }
    }

    public void DropGold(Transform dropPosition)
    {
        if (ChanceMaker.GetThisChanceResult_Percentage(60))
        {
            DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
            int randomGold = Random.Range(1, 100 + 1);
            item.SetGold(randomGold);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
