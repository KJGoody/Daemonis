using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    private static StorePanel instance;
    public static StorePanel Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<StorePanel>();
            return instance;
        }
    }

    public CanvasGroup storePanel;
    [SerializeField]
    private Transform StoreView;

    public void _SelectTap(Transform MyTransform)
    {
        MyTransform.SetSiblingIndex(3);
        StoreView.SetSiblingIndex(2);
    }
}
