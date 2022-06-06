using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOptionManager : MonoBehaviour
{
        
    private static AddOptionManager instance;
    public static AddOptionManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AddOptionManager>();
            }
            return instance;
        }
    }

    // ������� Ȯ��ǥ
    List<Dictionary<string, object>> tierProb; // �ɼ� Ƽ�� Ȯ��ǥ
    List<Dictionary<string, object>> valueProb; // �ɼǰ� Ȯ��ǥ
    
    float[] equipmentQualityProb = new float[]{ 5f, 4f, 3f, 2f, 1f, 0.5f};
    void Start()
    {
        valueProb = CSVReader.Read("AddOptionValueProb");
        tierProb = CSVReader.Read("OptionTierProb");
    }

    float[] GetTierProp(Quality quality) // ������ Ƽ� ���� �ɼ� Ƽ��Ȯ�� ��������;
    {
        float[] addOptionTierProp = new float[6];
        int itemTierNum = (int)quality;

        int a = 0;
        foreach (var value in tierProb[itemTierNum].Values)
        {
            addOptionTierProp[a] = (float)System.Convert.ToDouble(value);
            a++;
        }

        return addOptionTierProp;
    }

    public int SetRandomEquipmentQuality() // ��� ����Ƽ ����
    {
        return (int)ChanceMaker.Choose(equipmentQualityProb);
    }
    public int SetRandomTier(Quality quality) // �ɼ� Ƽ�� ����
    {
        return (int)ChanceMaker.Choose(GetTierProp(quality));
    }
    public int SetRandomKind() // �ɼ� ���� ����
    {
        return Random.Range(0, 22);
    }
    public float SetRandomValue(AddOption option) // �ɼ� ��ġ ����
    {
        float min = (float)System.Convert.ToDouble(valueProb[option.option_Num]["Tier"+option.tier+"_Min"]);
        float max = (float)System.Convert.ToDouble(valueProb[option.option_Num]["Tier" + option.tier + "_Max"]);

        return Random.Range(min, max);
    }
    public string GetOptionString(int optionNum) // �ɼ� ������ �ޱ�
    {
        return (string)valueProb[optionNum]["Option_String"];
    }
    public string GetOptionName(int optionNum) // �ɼ� �ѱ۹��ڿ� �ޱ�
    {
        return (string)valueProb[optionNum]["Option_Name"];
    }

    public int GetOptionNum(string optionName) // �ɼ� string�� int�� ��ȯ
    {
        int num = 0;
        switch (optionName)
        {
            case "":
                num = 0;
                break;
        }

        return num;
    }
}
