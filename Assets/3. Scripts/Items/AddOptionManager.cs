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
    //List<Dictionary<string, object>> kindProb; // 옵션 종류 확률표
    List<Dictionary<string, object>> tierProb; // 옵션 티어 확률표
    List<Dictionary<string, object>> valueProb; // 옵션값 확률표
    
    float[] tierProbx = new float[]{ 5, 4, 3f, 2f, 1f, 0.5f};
    void Start()
    {
        valueProb = CSVReader.Read("AddOptionValueProb");
        tierProb = CSVReader.Read("OptionTierProb");
        Debug.Log(tierProb[0].Keys);
        Debug.Log(tierProb[0].Keys.Count);
        
        int a = 0;
        foreach(var value in tierProb[3].Values) 
        {
            tierProbx[a] = (float)System.Convert.ToDouble(value);   
            a++;
        }
        
        
        for(int i=0; i<10000; i++)
        {
            Debug.Log(Choose(tierProbx));
        }
    }

    float Choose(float[] probs) // 가중치 랜덤뽑기
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

    float[] GetTierProp(ItemBase item) // 아이템 티어를 토대로 옵션 티어확률 가져오기;
    {
        float[] addOptionTierProp = new float[6];
        int itemTierNum = (int)item.MyQuality;

        int a = 0;
        foreach (var value in tierProb[itemTierNum].Values)
        {
            tierProbx[a] = (float)System.Convert.ToDouble(value);
            a++;
        }

        return addOptionTierProp;
    }


    public void GetRandomTier(ItemBase item)
    {
        Debug.Log(Choose(GetTierProp(item)));
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
