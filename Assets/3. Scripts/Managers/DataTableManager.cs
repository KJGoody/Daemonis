using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableManager : MonoBehaviour
{
    private static DataTableManager instance;
    public static DataTableManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DataTableManager>();
            return instance;
        }
    }

    [SerializeField]
    private DataTable_Item_Equipment dataTable_Item_Equipment;
    public DataTable_Item_Equipment GetDataTable_Item_Equipment { get { return dataTable_Item_Equipment; } }
}
