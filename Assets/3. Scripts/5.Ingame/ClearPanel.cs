using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPanel : MonoBehaviour
{
    private static ClearPanel instance;
    public static ClearPanel Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ClearPanel>();
            return instance;
        }
    }

    public void ClearGame()
    {

    }
}
