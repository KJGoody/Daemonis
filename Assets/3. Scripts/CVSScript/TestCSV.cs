using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCSV : MonoBehaviour
{
    private void Awake()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Test");

        //for (var i = 0; i < data.Count; i++)
        //{
        //    print(data[i]["ID"] + " " +
        //          data[i]["Name"] + " " +
        //          data[i]["Prefab"] + " " +
        //          data[i]["SpellType"] + " " +
        //          data[i]["Icon"] + " " +
        //          data[i]["Description"] + " " +
        //          data[i]["CoolTime"] + " " +
        //          data[i]["ManaCost"]);
        //}
    }
}