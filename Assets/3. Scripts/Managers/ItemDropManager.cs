using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Items //�࿡ �ش�Ǵ� �̸�
{
    public ItemInfo[] items;
}

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
    
    public DropItem dropItem; // ��������� ������
    public Items[] equipmentPerLv; // �⺻ ��� ������ ����Ʈ ���� �ش�Ǵ� �̸�
    public ItemInfo potion;

    private float equipmentDropProb = 10; // ��� ��� �⺻Ȯ��
    public float EquipmentDropProb // ��� ���Ȯ��
    {
        get { return equipmentDropProb + equipmentDropProb * Player.MyInstance.MyStat.ItemDropPercent; }
    }
    private int baseGold = 100; // ��� �⺻ ȹ�淮

    List<Dictionary<string, object>> qualityProb; // ��� ��� Ȯ��ǥ

    private void Start()
    {
        qualityProb = CSVReader.Read("EquipmentQualityProb"); // ��� ��� Ȯ��ǥ �о��
        StartCoroutine(InitItem());
    }

    public void DropGold(Transform dropPosition, int m_Level)
    {   // Enemy���� ��带 ����ϰ� �ϴ� �Լ�
        if (ChanceMaker.GetThisChanceResult_Percentage(60))
        {
            DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
            // ��差 ���� (((���� ���� * �⺻���) * ��ȹ ����) * ��������0.9~1.1)
            int randomGold = (int)(((m_Level * baseGold) + (m_Level * baseGold * (Player.MyInstance.MyStat.GoldPlus / 100))) * Random.Range(0.9f, 1.1f));
            item.SetGold(randomGold);
        }
    }

    public void DropItem(Transform dropPosition, int m_Level)
    {   // Enemy���� ��� �������� ����ϰ� �ϴ� �Լ�
        if (ChanceMaker.GetThisChanceResult_Percentage(EquipmentDropProb)) // ��� ���Ȯ�� ���ؼ� ��� ���
            DropEquipment(dropPosition, m_Level);

        if (ChanceMaker.GetThisChanceResult_Percentage(20)) // ���� ���
            DropPotion(dropPosition, m_Level);
    }

    public void DropEquipment(Transform dropPosition, int m_Level)  // ��� ����Լ�
    {
        // ������ ������ ����
        DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
        // ��� ���� ����
        int setKind = Random.Range(0, 42);

        // ��� ����
        float[] myQualityProb = new float[6];
        int a = 0;
        foreach (var value in qualityProb[SetLvNum(m_Level)].Values) // �������� �ٸ� Ȯ���� ������ �����ͼ� �迭�� �Ҵ�
            myQualityProb[a++] = (float)System.Convert.ToDouble(value);
        Quality newQuality = (Quality)(int)ChanceMaker.Choose(myQualityProb); // �Ҵ�� Ȯ�� �迭�� ����ġ �����̱�� ��� ����
        m_Level = 0;
        item.SetDropItem(equipmentPerLv[SetLvNum(m_Level)].items[setKind], newQuality); // ������ ���� �����ۿ� �־��ֱ�

    }

    public void DropPotion(Transform dropPosition, int m_Level) // �ӽ�
    {
        DropItem item = Instantiate(dropItem, dropPosition.position + ((Vector3)Random.insideUnitCircle * 0.5f), Quaternion.identity).GetComponent<DropItem>();
        item.SetDropItem(potion, Quality.Normal); 
    }

    public int SetLvNum(int monsterLv) // ���� ������ ���� ���� ����ֱ�
    {
        if (monsterLv > 50)
            monsterLv = 50;
        int levelNum = monsterLv / 10;
        return levelNum;
    }
    
    private IEnumerator InitItem()
    {
        yield return new WaitForSeconds(0.1f);

        ItemBase[] items = new ItemBase[4];

        for (int i = 0; i < 4; i++)
        {
            items[i] = new ItemBase();
            items[i].itemInfo = equipmentPerLv[0].items[i];
            Player.MyInstance.EquipItem(items[i]);
        }
        Player.MyInstance.MyStat.CurrentHealth = Player.MyInstance.MyStat.CurrentMaxHealth;
        Player.MyInstance.MyStat.CurrentMana = Player.MyInstance.MyStat.CurrentMaxMana;
    }
}
