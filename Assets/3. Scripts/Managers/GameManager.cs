using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    private INpc currentTarget;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        LoadData();
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
                currentTarget = hit.collider.GetComponent<INpc>();
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
        // Equipment 저장 부분
        for (int i = 0; i < 6; i++)
        {
            if (Player.MyInstance.usingEquipment[i] != null)
            {
                Item_Equipment equipment = Player.MyInstance.usingEquipment[i];
                DATA.EquipmentData[i] = equipment.GetJustName;
                DATA.EquipmentQuality[i] = (int)equipment.quality;

                if ((int)equipment.quality > 0)
                {
                    // AddOption을 하나하나 저장해주는 부분
                    int[] AddOptionQuality = DATA.GetArray("E_AddOptionQuality_" + i) as int[];
                    int[] AddOptionNum = DATA.GetArray("E_AddOptionNum_" + i) as int[];
                    float[] AddOptionValue = DATA.GetArray("E_AddOptionValue_" + i) as float[];

                    // 나오는 것은 List에서는 반대로 저장이 되어 있으므로 반대로 저장해줌
                    for (int j = DATA.EquipmentQuality[i]; j >= 0; j--)
                    {
                        AddOptionQuality[j] = equipment.addOptionList[j].Quality;
                        AddOptionNum[j] = equipment.addOptionList[j].Num;
                        AddOptionValue[j] = equipment.addOptionList[j].value;
                    }
                }
            }
            else
            {
                DATA.EquipmentData[i] = null;
                DATA.EquipmentQuality[i] = 0;

                int[] AddOptionQuality = DATA.GetArray("E_AddOptionQuality_" + i) as int[];
                AddOptionQuality = new int[6];
                int[] AddOptionNum = DATA.GetArray("E_AddOptionNum_" + i) as int[];
                AddOptionNum = new int[6];
                float[] AddOptionValue = DATA.GetArray("E_AddOptionValue_" + i) as float[];
                AddOptionValue = new float[6];
            }
        }

        // inventory 저장 부분
        // 인벤토리 1번부터 28번까지 전부 확인
        for (int i = 0; i < 28; i++)
        {
            // 만약 비어있지 않다면 저장 시작
            if (Slots[i].MyItem != null)
            {
                Item_Base SlotItem = Slots[i].MyItem;
                // 아이템의 순수 문자열을 저장
                DATA.InventoryData[i] = SlotItem.GetJustName;
                // 아이템의 퀄리티를 저장
                DATA.InventoryItemQuality[i] = (int)SlotItem.quality;

                // 아이템의 종류에 따라 다르게 저장
                switch (Slots[i].MyItem.GetKind)
                {
                    case ItemInfo_Base.Kinds.Equipment:
                        // 장비는 스텍이 필요 없으므로 로드할때에 아이템의 종류을 알수 있도록 -1로 설정한다.
                        DATA.InventoryStackData[i] = -1;
                        // 퀄리티가 일반이면 추가 옵션이 없으므로 조건문을 넣어줌
                        if ((int)Slots[i].MyItem.quality > 0)
                        {
                            // AddOption을 하나하나 저장해주는 부분
                            int[] AddOptionQuality = DATA.GetArray("I_AddOptionQuality_" + i) as int[];
                            int[] AddOptionNum = DATA.GetArray("I_AddOptionNum_" + i) as int[];
                            float[] AddOptionValue = DATA.GetArray("I_AddOptionValue_" + i) as float[];

                            // 나오는 것은 List에서는 반대로 저장이 되어 있으므로 반대로 저장해줌
                            for (int j = DATA.InventoryItemQuality[i]; j >= 0; j--)
                            {
                                Debug.Log(j);
                                AddOptionQuality[j] = (SlotItem as Item_Equipment).addOptionList[j].Quality;
                                AddOptionNum[j] = (SlotItem as Item_Equipment).addOptionList[j].Num;
                                AddOptionValue[j] = (SlotItem as Item_Equipment).addOptionList[j].value;
                            }
                        }
                        break;

                    case ItemInfo_Base.Kinds.Potion:
                        DATA.InventoryStackData[i] = Slots[i].MyItems.Count;
                        break;
                }
            }
            else
            {
                // 만약 아이템이 없다면 없는 부분을 초기화해주는 부분
                DATA.InventoryData[i] = null;
                DATA.InventoryItemQuality[i] = 0;
                DATA.InventoryStackData[i] = 0;

                int[] AddOptionQuality = DATA.GetArray("I_AddOptionQuality_" + i) as int[];
                AddOptionQuality = new int[6];
                int[] AddOptionNum = DATA.GetArray("I_AddOptionNum_" + i) as int[];
                AddOptionNum = new int[6];
                float[] AddOptionValue = DATA.GetArray("I_AddOptionValue_" + i) as float[];
                AddOptionValue = new float[6];
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
                DATA.ActionButtonsData[i] = (ActionButtons[i].GetUseableItem.Peek() as Item_Base).GetJustName;
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

        // Equipment 로드 부분
        for(int i = 0; i < 6; i++)
        {
            if(DATA.EquipmentData[i] != null)
            {
                if(DataTableManager.Instance.GetItemInfo_Equipment(DATA.EquipmentData[i]) != null)
                {
                    Item_Equipment DataItem = new Item_Equipment();
                    DataItem.itemInfo = DataTableManager.Instance.GetItemInfo_Equipment(DATA.EquipmentData[i]);
                    DataItem.quality = (Item_Base.Quality)DATA.EquipmentQuality[i];
                    for (int j = 0; j < (int)DataItem.quality + 1; j++)
                    {
                        int[] AddOptionQuality = DATA.GetArray("E_AddOptionQuality_" + i) as int[];
                        int[] AddOptionNum = DATA.GetArray("E_AddOptionNum_" + i) as int[];
                        float[] AddOptionValue = DATA.GetArray("E_AddOptionValue_" + i) as float[];
                        DataItem.addOptionList.Add(new ItemAddOption(AddOptionQuality[j], AddOptionNum[j], AddOptionValue[j]));
                    }
                    Player.MyInstance.EquipItem(DataItem);
                }
                else
                {
                    DATA.EquipmentData[i] = null;
                    DATA.EquipmentQuality[i] = 0;

                    int[] AddOptionQuality = DATA.GetArray("I_AddOptionQuality_" + i) as int[];
                    AddOptionQuality = new int[6];
                    int[] AddOptionNum = DATA.GetArray("I_AddOptionNum_" + i) as int[];
                    AddOptionNum = new int[6];
                    float[] AddOptionValue = DATA.GetArray("I_AddOptionValue_" + i) as float[];
                    AddOptionValue = new float[6];
                }
            }
            else
            {
                DATA.EquipmentData[i] = null;
                DATA.EquipmentQuality[i] = 0;

                int[] AddOptionQuality = DATA.GetArray("I_AddOptionQuality_" + i) as int[];
                AddOptionQuality = new int[6];
                int[] AddOptionNum = DATA.GetArray("I_AddOptionNum_" + i) as int[];
                AddOptionNum = new int[6];
                float[] AddOptionValue = DATA.GetArray("I_AddOptionValue_" + i) as float[];
                AddOptionValue = new float[6];
            }
        }

        // inventory 저장 부분
        for (int i = 0; i < 28; i++)
        {
            if (DATA.InventoryData[i] != null)
            {
                if (DATA.InventoryStackData[i] < 0)
                {
                    if (DataTableManager.Instance.GetItemInfo_Equipment(DATA.InventoryData[i]) != null)
                    {
                        Item_Equipment DataItem = new Item_Equipment();
                        DataItem.itemInfo = DataTableManager.Instance.GetItemInfo_Equipment(DATA.InventoryData[i]);
                        DataItem.quality = (Item_Base.Quality)DATA.InventoryItemQuality[i];
                        for (int j = 0; j < (int)DataItem.quality + 1; j++)
                        {
                            int[] AddOptionQuality = DATA.GetArray("I_AddOptionQuality_" + i) as int[];
                            int[] AddOptionNum = DATA.GetArray("I_AddOptionNum_" + i) as int[];
                            float[] AddOptionValue = DATA.GetArray("I_AddOptionValue_" + i) as float[];
                            DataItem.addOptionList.Add(new ItemAddOption(AddOptionQuality[j], AddOptionNum[j], AddOptionValue[j]));
                        }

                        Slots[i].AddItem(DataItem);
                        InventoryScript.MyInstance.OnItemCountChanged(DataItem);
                    }
                    else
                    {
                        DATA.InventoryData[i] = null;
                        DATA.InventoryItemQuality[i] = 0;
                        DATA.InventoryStackData[i] = 0;

                        int[] AddOptionQuality = DATA.GetArray("I_AddOptionQuality_" + i) as int[];
                        AddOptionQuality = new int[6];
                        int[] AddOptionNum = DATA.GetArray("I_AddOptionNum_" + i) as int[];
                        AddOptionNum = new int[6];
                        float[] AddOptionValue = DATA.GetArray("I_AddOptionValue_" + i) as float[];
                        AddOptionValue = new float[6];
                    }
                }
                else if (DATA.InventoryStackData[i] > 0)
                {
                    if (DataTableManager.Instance.GetItemInfo_Consumable(DATA.InventoryData[i]) != null)
                    {
                        Item_Consumable DataItem = new Item_Consumable();
                        DataItem.itemInfo = DataTableManager.Instance.GetItemInfo_Consumable(DATA.InventoryData[i]);
                        for (int j = 0; j < DATA.InventoryStackData[i]; j++)
                        {
                            Slots[i].AddItem(DataItem);
                            InventoryScript.MyInstance.OnItemCountChanged(DataItem);
                        }
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

                    int[] AddOptionQuality = DATA.GetArray("I_AddOptionQuality_" + i) as int[];
                    AddOptionQuality = new int[6];
                    int[] AddOptionNum = DATA.GetArray("I_AddOptionNum_" + i) as int[];
                    AddOptionNum = new int[6];
                    float[] AddOptionValue = DATA.GetArray("I_AddOptionValue_" + i) as float[];
                    AddOptionValue = new float[6];
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
            if (DATA.ActionButtonsData[i] != null)
            {
                if (DataTableManager.Instance.GetItemInfo_Consumable(DATA.ActionButtonsData[i]) != null)
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
    public string[] EquipmentData;
    public int[] EquipmentQuality;
    #region E_AddOptionQuality
    public int[] E_AddOptionQuality_0;
    public int[] E_AddOptionQuality_1;
    public int[] E_AddOptionQuality_2;
    public int[] E_AddOptionQuality_3;
    public int[] E_AddOptionQuality_4;
    public int[] E_AddOptionQuality_5;
    #endregion
    #region E_AddOptionNum
    public int[] E_AddOptionNum_0;
    public int[] E_AddOptionNum_1;
    public int[] E_AddOptionNum_2;
    public int[] E_AddOptionNum_3;
    public int[] E_AddOptionNum_4;
    public int[] E_AddOptionNum_5;
    #endregion
    #region E_AddOptionValue
    public float[] E_AddOptionValue_0;
    public float[] E_AddOptionValue_1;
    public float[] E_AddOptionValue_2;
    public float[] E_AddOptionValue_3;
    public float[] E_AddOptionValue_4;
    public float[] E_AddOptionValue_5;
    #endregion

    public int Gold;

    public string[] ActionButtonsData;

    public string[] InventoryData;
    public int[] InventoryItemQuality;
    public int[] InventoryStackData;
    // 아이템 AddOption 저장 부분
    #region I_AddOptionQuality
    public int[] I_AddOptionQuality_0;
    public int[] I_AddOptionQuality_1;
    public int[] I_AddOptionQuality_2;
    public int[] I_AddOptionQuality_3;
    public int[] I_AddOptionQuality_4;
    public int[] I_AddOptionQuality_5;
    public int[] I_AddOptionQuality_6;
    public int[] I_AddOptionQuality_7;
    public int[] I_AddOptionQuality_8;
    public int[] I_AddOptionQuality_9;
    public int[] I_AddOptionQuality_10;
    public int[] I_AddOptionQuality_11;
    public int[] I_AddOptionQuality_12;
    public int[] I_AddOptionQuality_13;
    public int[] I_AddOptionQuality_14;
    public int[] I_AddOptionQuality_15;
    public int[] I_AddOptionQuality_16;
    public int[] I_AddOptionQuality_17;
    public int[] I_AddOptionQuality_18;
    public int[] I_AddOptionQuality_19;
    public int[] I_AddOptionQuality_20;
    public int[] I_AddOptionQuality_21;
    public int[] I_AddOptionQuality_22;
    public int[] I_AddOptionQuality_23;
    public int[] I_AddOptionQuality_24;
    public int[] I_AddOptionQuality_25;
    public int[] I_AddOptionQuality_26;
    public int[] I_AddOptionQuality_27;
    #endregion
    #region I_AddOptionNum
    public int[] I_AddOptionNum_0;
    public int[] I_AddOptionNum_1;
    public int[] I_AddOptionNum_2;
    public int[] I_AddOptionNum_3;
    public int[] I_AddOptionNum_4;
    public int[] I_AddOptionNum_5;
    public int[] I_AddOptionNum_6;
    public int[] I_AddOptionNum_7;
    public int[] I_AddOptionNum_8;
    public int[] I_AddOptionNum_9;
    public int[] I_AddOptionNum_10;
    public int[] I_AddOptionNum_11;
    public int[] I_AddOptionNum_12;
    public int[] I_AddOptionNum_13;
    public int[] I_AddOptionNum_14;
    public int[] I_AddOptionNum_15;
    public int[] I_AddOptionNum_16;
    public int[] I_AddOptionNum_17;
    public int[] I_AddOptionNum_18;
    public int[] I_AddOptionNum_19;
    public int[] I_AddOptionNum_20;
    public int[] I_AddOptionNum_21;
    public int[] I_AddOptionNum_22;
    public int[] I_AddOptionNum_23;
    public int[] I_AddOptionNum_24;
    public int[] I_AddOptionNum_25;
    public int[] I_AddOptionNum_26;
    public int[] I_AddOptionNum_27;
    #endregion
    #region I_AddOptionValue
    public float[] I_AddOptionValue_0;
    public float[] I_AddOptionValue_1;
    public float[] I_AddOptionValue_2;
    public float[] I_AddOptionValue_3;
    public float[] I_AddOptionValue_4;
    public float[] I_AddOptionValue_5;
    public float[] I_AddOptionValue_6;
    public float[] I_AddOptionValue_7;
    public float[] I_AddOptionValue_8;
    public float[] I_AddOptionValue_9;
    public float[] I_AddOptionValue_10;
    public float[] I_AddOptionValue_11;
    public float[] I_AddOptionValue_12;
    public float[] I_AddOptionValue_13;
    public float[] I_AddOptionValue_14;
    public float[] I_AddOptionValue_15;
    public float[] I_AddOptionValue_16;
    public float[] I_AddOptionValue_17;
    public float[] I_AddOptionValue_18;
    public float[] I_AddOptionValue_19;
    public float[] I_AddOptionValue_20;
    public float[] I_AddOptionValue_21;
    public float[] I_AddOptionValue_22;
    public float[] I_AddOptionValue_23;
    public float[] I_AddOptionValue_24;
    public float[] I_AddOptionValue_25;
    public float[] I_AddOptionValue_26;
    public float[] I_AddOptionValue_27;
    #endregion

    public SaveLoadData()
    {
        EquipmentData = new string[6];
        EquipmentQuality = new int[6];
        #region E_AddOptionQuality
        E_AddOptionQuality_0 = new int[6];
        E_AddOptionQuality_1 = new int[6];
        E_AddOptionQuality_2 = new int[6];
        E_AddOptionQuality_3 = new int[6];
        E_AddOptionQuality_4 = new int[6];
        E_AddOptionQuality_5 = new int[6];
        #endregion
        #region E_AddOptionNum
        E_AddOptionNum_0 = new int[6];
        E_AddOptionNum_1 = new int[6];
        E_AddOptionNum_2 = new int[6];
        E_AddOptionNum_3 = new int[6];
        E_AddOptionNum_4 = new int[6];
        E_AddOptionNum_5 = new int[6];
        #endregion
        #region E_AddOptionValue
        E_AddOptionValue_0 = new float[6];
        E_AddOptionValue_1 = new float[6];
        E_AddOptionValue_2 = new float[6];
        E_AddOptionValue_3 = new float[6];
        E_AddOptionValue_4 = new float[6];
        E_AddOptionValue_5 = new float[6];
        #endregion

        Gold = 0;

        ActionButtonsData = new string[9];

        InventoryData = new string[28];
        InventoryItemQuality = new int[28];
        InventoryStackData = new int[28];
        #region I_AddOptionQuality
        I_AddOptionQuality_0 = new int[6];
        I_AddOptionQuality_1 = new int[6];
        I_AddOptionQuality_2 = new int[6];
        I_AddOptionQuality_3 = new int[6];
        I_AddOptionQuality_4 = new int[6];
        I_AddOptionQuality_5 = new int[6];
        I_AddOptionQuality_6 = new int[6];
        I_AddOptionQuality_7 = new int[6];
        I_AddOptionQuality_8 = new int[6];
        I_AddOptionQuality_9 = new int[6];
        I_AddOptionQuality_10 = new int[6];
        I_AddOptionQuality_11 = new int[6];
        I_AddOptionQuality_12 = new int[6];
        I_AddOptionQuality_13 = new int[6];
        I_AddOptionQuality_14 = new int[6];
        I_AddOptionQuality_15 = new int[6];
        I_AddOptionQuality_16 = new int[6];
        I_AddOptionQuality_17 = new int[6];
        I_AddOptionQuality_18 = new int[6];
        I_AddOptionQuality_19 = new int[6];
        I_AddOptionQuality_20 = new int[6];
        I_AddOptionQuality_21 = new int[6];
        I_AddOptionQuality_22 = new int[6];
        I_AddOptionQuality_23 = new int[6];
        I_AddOptionQuality_24 = new int[6];
        I_AddOptionQuality_25 = new int[6];
        I_AddOptionQuality_26 = new int[6];
        I_AddOptionQuality_27 = new int[6];
        #endregion
        #region I_AddOptionNum
        I_AddOptionNum_0 = new int[6];
        I_AddOptionNum_1 = new int[6];
        I_AddOptionNum_2 = new int[6];
        I_AddOptionNum_3 = new int[6];
        I_AddOptionNum_4 = new int[6];
        I_AddOptionNum_5 = new int[6];
        I_AddOptionNum_6 = new int[6];
        I_AddOptionNum_7 = new int[6];
        I_AddOptionNum_8 = new int[6];
        I_AddOptionNum_9 = new int[6];
        I_AddOptionNum_10 = new int[6];
        I_AddOptionNum_11 = new int[6];
        I_AddOptionNum_12 = new int[6];
        I_AddOptionNum_13 = new int[6];
        I_AddOptionNum_14 = new int[6];
        I_AddOptionNum_15 = new int[6];
        I_AddOptionNum_16 = new int[6];
        I_AddOptionNum_17 = new int[6];
        I_AddOptionNum_18 = new int[6];
        I_AddOptionNum_19 = new int[6];
        I_AddOptionNum_20 = new int[6];
        I_AddOptionNum_21 = new int[6];
        I_AddOptionNum_22 = new int[6];
        I_AddOptionNum_23 = new int[6];
        I_AddOptionNum_24 = new int[6];
        I_AddOptionNum_25 = new int[6];
        I_AddOptionNum_26 = new int[6];
        I_AddOptionNum_27 = new int[6];
        #endregion
        #region I_AddOptionValue
        I_AddOptionValue_0 = new float[6];
        I_AddOptionValue_1 = new float[6];
        I_AddOptionValue_2 = new float[6];
        I_AddOptionValue_3 = new float[6];
        I_AddOptionValue_4 = new float[6];
        I_AddOptionValue_5 = new float[6];
        I_AddOptionValue_6 = new float[6];
        I_AddOptionValue_7 = new float[6];
        I_AddOptionValue_8 = new float[6];
        I_AddOptionValue_9 = new float[6];
        I_AddOptionValue_10 = new float[6];
        I_AddOptionValue_11 = new float[6];
        I_AddOptionValue_12 = new float[6];
        I_AddOptionValue_13 = new float[6];
        I_AddOptionValue_14 = new float[6];
        I_AddOptionValue_15 = new float[6];
        I_AddOptionValue_16 = new float[6];
        I_AddOptionValue_17 = new float[6];
        I_AddOptionValue_18 = new float[6];
        I_AddOptionValue_19 = new float[6];
        I_AddOptionValue_20 = new float[6];
        I_AddOptionValue_21 = new float[6];
        I_AddOptionValue_22 = new float[6];
        I_AddOptionValue_23 = new float[6];
        I_AddOptionValue_24 = new float[6];
        I_AddOptionValue_25 = new float[6];
        I_AddOptionValue_26 = new float[6];
        I_AddOptionValue_27 = new float[6];
        #endregion
    }

    public object GetArray(string VariableName)
    {
        return GetType().GetField(VariableName).GetValue(this);
    }
}
