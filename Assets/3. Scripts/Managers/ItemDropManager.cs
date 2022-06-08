using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Items //행에 해당되는 이름
{
    public Item[] items;
}
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
    public DropItem dropItem; // 드랍아이템 프리팹
    public Items[] equipmentPerLv; // 기본 장비 아이템 리스트 열에 해당되는 이름
    public Item potion;

    private float equipmentDropProb = 10; // 장비 드랍 기본확률
    private int baseGold = 100; // 골드 기본 획득량
    public float EquipmentDropProb // 장비 드랍확률
    {
        get { return equipmentDropProb + equipmentDropProb * Player.MyInstance.MyStat.ItemDropPercent; }
    }

    List<Dictionary<string, object>> qualityProb; // 장비 등급 확률표

    private void Start()
    {
        qualityProb = CSVReader.Read("EquipmentQualityProb"); // 장비 등급 확률표 읽어옴
        Invoke("TestDrop",1);
    }
    public void DropItem(Transform dropPosition, int m_Level)
    {
        if (ChanceMaker.GetThisChanceResult_Percentage(EquipmentDropProb)) // 장비 드랍확률 통해서 장비 드랍
        {
            DropEquipment(dropPosition, m_Level);
        }
        if (ChanceMaker.GetThisChanceResult_Percentage(20)) // 포션 드랍 (임시)
        {
            DropPotion(dropPosition, m_Level);
        }
    }

    public void DropGold(Transform dropPosition, int m_Level)
    {
        if (ChanceMaker.GetThisChanceResult_Percentage(60))
        {
            DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
            // 골드량 계산식 (((몬스터 레벨 * 기본골드) * 골획 배율) * 난수범위0.9~1.1)
            int randomGold = (int)(((m_Level * baseGold) + (m_Level * baseGold * (Player.MyInstance.MyStat.GoldPlus / 100))) * Random.Range(0.9f, 1.1f));
            item.SetGold(randomGold);
        }

    }

    public void DropEquipment(Transform dropPosition, int m_Level)  // 장비 드랍함수
    {
        // 아이템 프리팹 생성
        DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
        // 장비 종류 설정
        int setKind = Random.Range(0, 6);

        // 등급 설정
        float[] myQualityProb = new float[6];
        int a = 0;
        foreach (var value in qualityProb[SetLvNum(m_Level)].Values) // 레벨마다 다른 확률을 엑셀로 가져와서 배열에 할당
        {
            myQualityProb[a] = (float)System.Convert.ToDouble(value);
            a++;
        }
        Quality newQuality = (Quality)(int)ChanceMaker.Choose(myQualityProb); // 할당된 확률 배열로 가중치 랜덤뽑기로 등급 설정
        m_Level = 0;
        item.SetDropItem(equipmentPerLv[SetLvNum(m_Level)].items[setKind], newQuality); // 설정한 정보 아이템에 넣어주기

    }
    public void DropPotion(Transform dropPosition, int m_Level) // 임시
    {
        DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
        item.SetDropItem(potion, Quality.Normal); 

    }
    public int SetLvNum(int monsterLv) // 몬스터 레벨로 설정 레벨 잡아주기
    {
        if (monsterLv > 50)
            monsterLv = 50;
        int levelNum = monsterLv / 10;
        return levelNum;
    }
    public void TestDrop()
    {
        for (int i = 0; i < 6; i++)
        {
            DropItem item = Instantiate(dropItem, transform.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
            
            int m_Level = 0;
            item.SetDropItem(equipmentPerLv[SetLvNum(m_Level)].items[i], (Quality)(int)Random.Range(0,4));
        }
    }
    
}
