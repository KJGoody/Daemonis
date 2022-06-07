using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public SaveLoadData SavedData { get; private set; }
    public SaveLoadData DATA;

    [SerializeField]
    private Player player;

    private NPC currentTarget;

    private void Awake()
    {
        LoadData();
    }

    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));
            if (hit.collider != null)
            {
                if (currentTarget != null)
                    currentTarget.DeSelect();
                currentTarget = hit.collider.GetComponent<NPC>();
                player.MyTarget = currentTarget.Select();
            }
            else
            {
                if (currentTarget != null)
                    currentTarget.DeSelect();
                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }

    public void SaveData()
    {
        // 지금까지의 변경사항을 저장한다.
        SaveLoadManager.DataSave(DATA, "Data");
    }

    public void LoadData()
    {
        if (SaveLoadManager.FileExists("Data"))
            SavedData = SaveLoadManager.DataLoad<SaveLoadData>("Data");
        else
            SavedData = new SaveLoadData();

        // 저장되어있는 사항을 저장한다.
        DATA = SavedData;
    }
}

[System.Serializable]
public class SaveLoadData
{
    public int Gold;

    public SaveLoadData()
    {
        Gold = 0;
    }
}
