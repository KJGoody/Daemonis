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

    // 엑셀기반 확률표
    List<Dictionary<string, object>> tierProb; // 옵션 티어 확률표
    List<Dictionary<string, object>> valueProb; // 옵션값 확률표
    
    float[] equipmentQualityProb = new float[]{ 5f, 4f, 3f, 2f, 1f, 0.5f};
    void Start()
    {
        valueProb = CSVReader.Read("AddOptionValueProb");
        tierProb = CSVReader.Read("OptionTierProb");
    }

    float[] GetTierProp(Quality quality) // 아이템 티어를 토대로 옵션 티어확률 가져오기;
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

    public int SetRandomEquipmentQuality() // 장비 퀄리티 랜덤
    {
        return (int)ChanceMaker.Choose(equipmentQualityProb);
    }
    public int SetRandomTier(Quality quality) // 옵션 티어 랜덤
    {
        return (int)ChanceMaker.Choose(GetTierProp(quality));
    }
    public int SetRandomKind() // 옵션 종류 랜덤
    {
        return Random.Range(0, 22);
    }
    public float SetRandomValue(AddOption option) // 옵션 수치 랜덤
    {
        float min = (float)System.Convert.ToDouble(valueProb[option.option_Num]["Tier"+option.tier+"_Min"]);
        float max = (float)System.Convert.ToDouble(valueProb[option.option_Num]["Tier" + option.tier + "_Max"]);

        return Random.Range(min, max);
    }
    public string GetOptionString(int optionNum) // 옵션 변수명 받기
    {
        return (string)valueProb[optionNum]["Option_String"];
    }
    public string GetOptionName(int optionNum) // 옵션 한글문자열 받기
    {
        return (string)valueProb[optionNum]["Option_Name"];
    }

    public int GetOptionNum(string optionName) // 옵션 string을 int로 변환
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
