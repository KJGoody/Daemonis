using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAddOption
{
    public int Quality;
    public int Num;
    public float value;

    public ItemAddOption(int AddOptionQuality, int AddOptionNum, float value)
    {
        this.Quality = AddOptionQuality;
        this.Num = AddOptionNum;
        this.value = value;
    }
}

public class ItemAddOptionScript : MonoBehaviour
{
    private static ItemAddOptionScript instance;
    public static ItemAddOptionScript Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ItemAddOptionScript>();
            return instance;
        }
    }

    private List<Dictionary<string, object>> QualityProbTable; // �ɼ� Ƽ�� Ȯ��ǥ
    private List<Dictionary<string, object>> ValueProbTable; // �ɼǰ� Ȯ��ǥ

    private void Start()
    {
        QualityProbTable = CSVReader.Read("OptionTierProb");
        ValueProbTable = CSVReader.Read("AddOptionValueProb");
    }

    public int SetRandomQuality(Item_Base.Quality quality)   // �߰� �ɼ��� ����� �����ϰ� ����
    {   
        return (int)ChanceMaker.Choose(GetAddOptionQualityProbTable(quality));
    }

    private float[] GetAddOptionQualityProbTable(Item_Base.Quality quality)   // �������� Ƽ� �������� �߰� �ɼ� ���Ȯ�� ��������
    {
        float[] AddOptionQualityPropTable = new float[6];
        int ItemQualityNum = (int)quality;

        int a = 0;
        foreach (var value in QualityProbTable[ItemQualityNum].Values)
            AddOptionQualityPropTable[a++] = (float)System.Convert.ToDouble(value);

        return AddOptionQualityPropTable;
    }

    public int SetRandomAddOption()
    {
        return Random.Range(0, 22);
    }

    public float SetRandomValue(ItemAddOption option) // �ɼ� ��ġ ����
    {
        float min = (float)System.Convert.ToDouble(ValueProbTable[option.Num]["Tier" + option.Quality + "_Min"]);
        float max = (float)System.Convert.ToDouble(ValueProbTable[option.Num]["Tier" + option.Quality + "_Max"]);

        return Random.Range(min, max);
    }

    public string GetNameString(int optionNum) // �ɼ� ������ �ޱ�
    {
        return (string)ValueProbTable[optionNum]["Option_String"];
    }

    public string GetName(int optionNum) // �ɼ� �ѱ۹��ڿ� �ޱ�
    {
        return (string)ValueProbTable[optionNum]["Option_Name"];
    }
}
