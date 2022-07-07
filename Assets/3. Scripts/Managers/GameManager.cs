using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // 페이드인 효과
    Color color;
    [SerializeField]
    Image fadeIn_IMG;
    [SerializeField]
    GameObject fadeIn_OBJ;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public SaveLoadData DATA;
    private SaveLoadData SavedData;
    public SaveLoadData GetSavedData { get { return SavedData; } }

    public ActionButton[] ActionButtons;
    public SlotScript[] Slots;

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject quitPanel;

    [SerializeField]
    private GameObject[] dontDestroyObj;
    private NPC currentTarget;

    private void Awake()
    {
        LoadData();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // 게임종료 열기
        {
            if (!quitPanel.activeSelf)
                quitPanel.SetActive(true);
            else
                quitPanel.SetActive(false);
        }
        ClickTarget();
    }

    private void ClickTarget() // 타겟 선택
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 로딩될때 실행
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn() // 검정색 페이드인
    {
        fadeIn_OBJ.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            fadeIn_IMG.color = color;
            yield return null;
        }

        if (fadeIn_IMG.color.a <= 0)
            fadeIn_OBJ.SetActive(false);

        color.a = 1;
        fadeIn_IMG.color = color;

    }
    public void GameQuit() //게임 종료
    {
        Application.Quit();
    }


    public void SaveData()
    {
        // inventory 저장 부분
        for (int i = 0; i < 28; i++)
        {
            if (Slots[i].MyItem != null)
            {
                DATA.InventoryData[i] = Slots[i].MyItem.GetJustName;

                switch (Slots[i].MyItem.GetKind)
                {
                    case ItemInfo_Base.Kinds.Equipment:
                        DATA.InventoryStackData[i] = -1;
                        break;

                    case ItemInfo_Base.Kinds.Potion:
                        DATA.InventoryStackData[i] = Slots[i].MyItems.Count;
                        break;
                }
            }
            else
            {
                DATA.InventoryData[i] = null;
                DATA.InventoryStackData[i] = 0;
            }
        }

        // ActionButton 저장 부분
        for (int i = 0; i < 5; i++)
        {
            if (ActionButtons[i].UseableSpell != null)
                DATA.ActionButtonsData[i] = ActionButtons[i].UseableSpell.GetName();
            else
                DATA.ActionButtonsData[i] = null;
        }
        for (int i = 5; i < 9; i++)
        {
            if (ActionButtons[i].GetUseableItem != null)
                DATA.ActionButtonsData[i] = ActionButtons[i].GetUseableItem.Peek().GetName();
            else
                DATA.ActionButtonsData[i] = null;
        }

        // 지금까지의 변경사항을 저장한다.
        SaveLoadManager.DataSave(DATA, "Data");
    }

    public void LoadData()
    {
        if (SaveLoadManager.FileExists("Data"))
            SavedData = SaveLoadManager.DataLoad<SaveLoadData>("Data");
        else
            SavedData = new SaveLoadData();

        // 저장되어있는 사항을 로드한다.
        DATA = SavedData;

        // inventory 저장 부분
        for (int i = 0; i < 28; i++)
        {
            if(DATA.InventoryData[i] != null)
            {
                if(DATA.InventoryStackData[i] < 0)
                {
                    if(DataTableManager.Instance.GetItemInfo_Equipment(DATA.InventoryData[i]) != null)
                    {
                        Item_Equipment DataItem = new Item_Equipment();
                        DataItem.itemInfo = DataTableManager.Instance.GetItemInfo_Equipment(DATA.InventoryData[i]);
                        InventoryScript.MyInstance.AddItem(DataItem);
                    }
                    else
                    {
                        DATA.InventoryData[i] = null;
                        DATA.InventoryStackData[i] = 0;
                    }
                }
                else if(DATA.InventoryStackData[i] > 0)
                {
                    if (DataTableManager.Instance.GetItemInfo_Consumable(DATA.InventoryData[i]) != null)
                    {
                        Item_Consumable DataItem = new Item_Consumable();
                        DataItem.itemInfo = DataTableManager.Instance.GetItemInfo_Consumable(DATA.InventoryData[i]);
                        for (int j = 0; j < DATA.InventoryStackData[i]; j++)
                            InventoryScript.MyInstance.AddItem(DataItem, true);
                    }
                    else
                    {
                        DATA.InventoryData[i] = null;
                        DATA.InventoryStackData[i] = 0;
                    }
                }
                else
                {
                    DATA.InventoryData[i] = null;
                    DATA.InventoryStackData[i] = 0;
                }
            }
        }

        // ActionButton 저장 부분
        for (int i = 0; i < 5; i++)
        {
            if (DATA.ActionButtonsData[i] != null)
            {
                if (DataTableManager.Instance.GetSpellData(DATA.ActionButtonsData[i]) != null)
                {
                    Spell DataSpell = new Spell();
                    DataSpell.spellInfo = DataTableManager.Instance.GetSpellData(DATA.ActionButtonsData[i]);
                    ActionButtons[i].SetUseable(DataSpell);
                }
                else
                    DATA.ActionButtonsData[i] = null;
            }
        }
        for (int i = 5; i < 9; i++)
        {
            if(DATA.ActionButtonsData[i] != null)
            {
                if(DataTableManager.Instance.GetItemInfo_Consumable(DATA.ActionButtonsData[i]) != null)
                {
                    Item_Consumable Dataitem = new Item_Consumable();
                    Dataitem.itemInfo = DataTableManager.Instance.GetItemInfo_Consumable(DATA.ActionButtonsData[i]);
                    ActionButtons[i].SetUseable(Dataitem);
                }
                else
                    DATA.ActionButtonsData[i] = null;
            }
        }
    }
}

[System.Serializable]
public class SaveLoadData
{
    public int Gold;

    public string[] ActionButtonsData;
    public string[] InventoryData;
    public int[] InventoryStackData;



    public SaveLoadData()
    {
        Gold = 0;
        ActionButtonsData = new string[9];
        InventoryData = new string[28];
        InventoryStackData = new int[28];
    }
}
