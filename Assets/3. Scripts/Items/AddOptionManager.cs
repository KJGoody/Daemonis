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
    //List<Dictionary<string, object>> kindProb; // �ɼ� ���� Ȯ��ǥ
    List<Dictionary<string, object>> tierProb; // �ɼ� Ƽ�� Ȯ��ǥ
    List<Dictionary<string, object>> valueProb; // �ɼǰ� Ȯ��ǥ
    
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

    float Choose(float[] probs) // ����ġ �����̱�
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

    float[] GetTierProp(ItemBase item) // ������ Ƽ� ���� �ɼ� Ƽ��Ȯ�� ��������;
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
