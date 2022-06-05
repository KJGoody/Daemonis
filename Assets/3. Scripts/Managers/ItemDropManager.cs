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
    public Items[] equipmentPerLv; // 기본 장비 아이템 리스트 열에 해당되는 이름
    public DropItem dropItem; // 드랍아이템 프리팹

    private float equipmentDropProb = 10;
    public float EquipmentDropProb // 장비 드랍확률
    {
        get { return equipmentDropProb + equipmentDropProb * Player.MyInstance.MyStat.ItemDropPercent; }
    }

    List<Dictionary<string, object>> qualityProb; // 장비 등급 확률표

    private void Start()
    {
        qualityProb = CSVReader.Read("EquipmentQualityProb");
    }
    public void DropItem(Transform dropPosition, int m_Level)
    {
        if (ChanceMaker.GetThisChanceResult_Percentage(EquipmentDropProb)) // 장비 드랍확률 통해서 장비 드랍
        {
            DropEquipment(dropPosition, m_Level);
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
        item.SetDropItem(equipmentPerLv[SetLvNum(m_Level)].items[setKind], newQuality); // 설정한 정보 아이템에 넣어주기

    }
    public int SetLvNum(int monsterLv) // 몬스터 레벨로 설정 레벨 잡아주기
    {
        if (monsterLv > 50)
            monsterLv = 50;
        int levelNum = monsterLv / 10;
        return levelNum;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(equipmentPerLv[0].items[0]);
        }
    }
}
