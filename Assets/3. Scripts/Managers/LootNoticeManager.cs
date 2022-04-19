using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootNoticeManager : MonoBehaviour
{
    private static LootNoticeManager instance;
    public static LootNoticeManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LootNoticeManager>();
            }

            return instance;
        }
    }
    public List<GameObject> noticeList = new List<GameObject>();
    [SerializeField]
    private ContentSizeFitter noticeManagerPanel;
    // Update is called once per frame
    void Update()
    {
        if (noticeList.Count >= 4)
        {
            DeleteLootNotice();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)noticeManagerPanel.transform);
    }
    public void AddLootNotice(GameObject notice)
    {
        noticeList.Add(notice);
    }
    public void DeleteLootNotice()
    {
        LootNotice a = noticeList[0].GetComponent<LootNotice>();
        a.Off();
    }

}
