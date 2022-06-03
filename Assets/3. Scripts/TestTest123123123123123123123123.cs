using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest123123123123123123123123 : MonoBehaviour
{
    public int _exp = 0;

    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("AddOptionTest");

        for (var i = 0; i < data.Count; i++)
        {
            Debug.Log("index " + (i).ToString() + " : " + data[i]["Option_Name"] + " " + data[i]["Tier0_Min"].ToString() + " " + data[i]["Tier0_Max"].ToString());
        }

    }

}
