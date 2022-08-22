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
    
    public ItemCart dropItem; // ��������� ������

    private float equipmentDropProb = 10; // ��� ��� �⺻Ȯ��
    private float EquipmentDropProb // ��� ���Ȯ��
    {
        get { return equipmentDropProb + equipmentDropProb * Player.MyInstance.MyStat.ItemDropPercent; }
    }
    private int baseGold = 100; // ��� �⺻ ȹ�淮

    public void DropGold(Transform dropPosition, int m_Level)
    {   
        if (ChanceMaker.GetThisChanceResult_Percentage(60))
        {
            ItemCart item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<ItemCart>();
            // ��差 ���� (((���� ���� * �⺻���) * ��ȹ ����) * ��������0.9~1.1)
            int randomGold = (int)(((m_Level * baseGold) + (m_Level * baseGold * (Player.MyInstance.MyStat.GoldPlus / 100))) * Random.Range(0.9f, 1.1f));
            item.SetGold(randomGold);
        }
    }

    public void DropItem(Transform dropPosition, int m_Level)
    {   
        if (ChanceMaker.GetThisChanceResult_Percentage(EquipmentDropProb)) // ��� ���Ȯ�� ���ؼ� ��� ���
            DropEquipment(dropPosition, m_Level);

        if (ChanceMaker.GetThisChanceResult_Percentage(20)) // ���� ���
            DropPotion(dropPosition, m_Level);
    }

    public void DropEquipment(Transform dropPosition, int m_Level)  // ��� ����Լ�
    {
        // ������ ������ ����
        ItemCart item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<ItemCart>();
        item.SetItem_Equipment(DataTableManager.Instance.GetItemInfo_Equipment(m_Level), DataTableManager.Instance.GetQuality(m_Level)); // ������ ���� �����ۿ� �־��ֱ�
    }

    public void DropPotion(Transform dropPosition, int m_Level)
    {
        ItemCart item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<ItemCart>();
        item.SetItem_Consumable(DataTableManager.Instance.GetItemInfo_Consumable(m_Level), Item_Base.Qualitys.Normal); 
    }
}
