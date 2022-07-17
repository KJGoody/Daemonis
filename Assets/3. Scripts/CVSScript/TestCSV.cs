using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCSV : MonoBehaviour
{
    private void Awake()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Test");

        for (var i = 0; i < data.Count; i++)
        {
            print(data[i]["1"] + data[i]["1"].GetType().ToString() + " " +
                  data[i]["2"] + data[i]["2"].GetType().ToString() + " " +
                  data[i]["3"] + data[i]["3"].GetType().ToString() + " " +
                  data[i]["4"] + data[i]["4"].GetType().ToString() + " " +
                  data[i]["5"] + data[i]["5"].GetType().ToString());
        }

    }
}