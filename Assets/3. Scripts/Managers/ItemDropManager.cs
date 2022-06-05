using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Items //�࿡ �ش�Ǵ� �̸�
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
    public Items[] equipmentPerLv; // �⺻ ��� ������ ����Ʈ ���� �ش�Ǵ� �̸�
    public DropItem dropItem; // ��������� ������

    private float equipmentDropProb = 10;
    public float EquipmentDropProb // ��� ���Ȯ��
    {
        get { return equipmentDropProb + equipmentDropProb * Player.MyInstance.MyStat.ItemDropPercent; }
    }

    List<Dictionary<string, object>> qualityProb; // ��� ��� Ȯ��ǥ

    private void Start()
    {
        qualityProb = CSVReader.Read("EquipmentQualityProb");
    }
    public void DropItem(Transform dropPosition, int m_Level)
    {
        if (ChanceMaker.GetThisChanceResult_Percentage(EquipmentDropProb)) // ��� ���Ȯ�� ���ؼ� ��� ���
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

    public void DropEquipment(Transform dropPosition, int m_Level)  // ��� ����Լ�
    {
        // ������ ������ ����
        DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
        // ��� ���� ����
        int setKind = Random.Range(0, 6);

        // ��� ����
        float[] myQualityProb = new float[6];
        int a = 0;
        foreach (var value in qualityProb[SetLvNum(m_Level)].Values) // �������� �ٸ� Ȯ���� ������ �����ͼ� �迭�� �Ҵ�
        {
            myQualityProb[a] = (float)System.Convert.ToDouble(value);
            a++;
        }
        Quality newQuality = (Quality)(int)ChanceMaker.Choose(myQualityProb); // �Ҵ�� Ȯ�� �迭�� ����ġ �����̱�� ��� ����
        item.SetDropItem(equipmentPerLv[SetLvNum(m_Level)].items[setKind], newQuality); // ������ ���� �����ۿ� �־��ֱ�

    }
    public int SetLvNum(int monsterLv) // ���� ������ ���� ���� ����ֱ�
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
