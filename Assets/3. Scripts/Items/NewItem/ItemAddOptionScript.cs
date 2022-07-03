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

    private List<Dictionary<string, object>> QualityProbTable; // 옵션 티어 확률표
    private List<Dictionary<string, object>> ValueProbTable; // 옵션값 확률표

    private void Start()
    {
        QualityProbTable = CSVReader.Read("OptionTierProb");
        ValueProbTable = CSVReader.Read("AddOptionValueProb");
    }

    public int SetRandomQuality(Item_Base.Quality quality)   // 추가 옵션의 등급을 랜덤하게 설정
    {   
        return (int)ChanceMaker.Choose(GetAddOptionQualityProbTable(quality));
    }

    private float[] GetAddOptionQualityProbTable(Item_Base.Quality quality)   // 아이템의 티어를 바탕으로 추가 옵션 등급확률 가져오기
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

    public float SetRandomValue(ItemAddOption option) // 옵션 수치 랜덤
    {
        float min = (float)System.Convert.ToDouble(ValueProbTable[option.Num]["Tier" + option.Quality + "_Min"]);
        float max = (float)System.Convert.ToDouble(ValueProbTable[option.Num]["Tier" + option.Quality + "_Max"]);

        return Random.Range(min, max);
    }

    public string GetNameString(int optionNum) // 옵션 변수명 받기
    {
        return (string)ValueProbTable[optionNum]["Option_String"];
    }

    public string GetName(int optionNum) // 옵션 한글문자열 받기
    {
        return (string)ValueProbTable[optionNum]["Option_Name"];
    }
}
