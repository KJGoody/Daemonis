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
                instance = FindObjectOfType<ItemDropManager>();
            return instance;
        }
    }
    
    private float equipmentDropProb = 1; // 장비 드랍 기본확률
    private float EquipmentDropProb // 장비 드랍확률
    {
        get { return equipmentDropProb + equipmentDropProb * Player.MyInstance.MyStat.ItemDropPercent; }
    }
    private int baseGold = 100; // 골드 기본 획득량

    public void DropGold(Transform dropPosition, int m_Level)
    {   
        if (ChanceMaker.GetThisChanceResult_Percentage(60))
        {
            ItemCart item = Instantiate(Resources.Load<GameObject>("Prefabs/P_DropItem"),
                dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f),
                Quaternion.identity).GetComponent<ItemCart>();
            // 골드량 계산식 (((몬스터 레벨 * 기본골드) * 골획 배율) * 난수범위0.9~1.1)
            int randomGold = (int)(((m_Level * baseGold) + (m_Level * baseGold * (Player.MyInstance.MyStat.GoldPlus / 100))) * Random.Range(0.9f, 1.1f));
            item.SetGold(randomGold);
        }
    }

    public void DropItem(Transform dropPosition, int m_Level)
    {   
        if (ChanceMaker.GetThisChanceResult_Percentage(EquipmentDropProb)) // 장비 드랍확률 통해서 장비 드랍
            DropEquipment(dropPosition, m_Level);

        if (ChanceMaker.GetThisChanceResult_Percentage(5)) // 포션 드랍
            DropPotion(dropPosition, m_Level);
    }

    public void DropEquipment(Transform dropPosition, int m_Level)  // 장비 드랍함수
    {
        // 아이템 프리팹 생성
        ItemCart item = Instantiate(Resources.Load<GameObject>("Prefabs/P_DropItem"),
            dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), 
            Quaternion.identity).GetComponent<ItemCart>();
        item.SetItem_Equipment(DataTableManager.Instance.GetInfo_Equipment(m_Level), DataTableManager.Instance.GetQuality(m_Level)); // 설정한 정보 아이템에 넣어주기
    }

    public void DropPotion(Transform dropPosition, int m_Level)
    {
        ItemCart item = Instantiate(Resources.Load<GameObject>("Prefabs/P_DropItem"),
            dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), 
            Quaternion.identity).GetComponent<ItemCart>();
        item.SetItem_Consumable(DataTableManager.Instance.GetInfo_Consumable(m_Level), Item_Base.Qualitys.Normal); 
    }
}
