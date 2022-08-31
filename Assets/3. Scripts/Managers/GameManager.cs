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
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    // 페이드인 효과
    Color color;
    [SerializeField] Image fadeIn_IMG;
    [SerializeField] GameObject fadeIn_OBJ;

    // 저장
    public SaveLoadData DATA;

    [HideInInspector] public string CurrnetStageName;

    public CastingButton[] CastingButtons;
    public QuickSlotButton[] QuickSlotButtons;
    public SlotScript[] Slots;

    [SerializeField] private Player player;
    private INpc currentTarget;
    [SerializeField] private GameObject quitPanel;

    [SerializeField] private GameObject[] dontDestroyObj;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 로딩될때 실행
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

    public void GameQuit() //게임 종료
    {
        Application.Quit();
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

    public void SaveData()
    {
        // Equipment 저장 부분
        for (int i = 0; i < 6; i++)
        {
            if (Player.MyInstance.usingEquipment[i] != null)
            {
                Item_Equipment equipment = Player.MyInstance.usingEquipment[i];
                DATA.E_Data[i] = equipment.ID;
                DATA.E_Quality[i] = (int)equipment.Quality;

                if ((int)equipment.Quality > 0)
                {
                    // AddOption을 하나하나 저장해주는 부분
                    int[] AddOptionQuality = DATA.GetArray("E_AddOptionQuality_" + i) as int[];
                    int[] AddOptionNum = DATA.GetArray("E_AddOptionNum_" + i) as int[];
                    float[] AddOptionValue = DATA.GetArray("E_AddOptionValue_" + i) as float[];

                    // 나오는 것은 List에서는 반대로 저장이 되어 있으므로 반대로 저장해줌
                    for (int j = DATA.E_Quality[i]; j >= 0; j--)
                    {
                        AddOptionQuality[j] = equipment.addOptionList[j].Quality;
                        AddOptionNum[j] = equipment.addOptionList[j].Num;
                        AddOptionValue[j] = equipment.addOptionList[j].value;
                    }
                }
            }
            else
            {
                DATA.E_Data[i] = null;
                DATA.E_Quality[i] = 0;
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
                // 아이템의 ID를 저장
                DATA.I_Data[i] = SlotItem.ID;
                // 아이템의 퀄리티를 저장
                DATA.I_ItemQuality[i] = (int)SlotItem.Quality;

                // 아이템의 종류에 따라 다르게 저장
                switch (Slots[i].MyItem.Kind)
                {
                    case ItemInfo_Base.Kinds.Equipment:
                        // 장비는 스텍이 필요 없으므로 로드할때에 아이템의 종류을 알수 있도록 -1로 설정한다.
                        DATA.I_StackData[i] = -1;
                        // 퀄리티가 일반이면 추가 옵션이 없으므로 조건문을 넣어줌
                        if ((int)Slots[i].MyItem.Quality > 0)
                        {
                            // AddOption을 하나하나 저장해주는 부분
                            int[] AddOptionQuality = DATA.GetArray("I_AddOptionQuality_" + i) as int[];
                            int[] AddOptionNum = DATA.GetArray("I_AddOptionNum_" + i) as int[];
                            float[] AddOptionValue = DATA.GetArray("I_AddOptionValue_" + i) as float[];

                            // 나오는 것은 List에서는 반대로 저장이 되어 있으므로 반대로 저장해줌
                            for (int j = DATA.I_ItemQuality[i]; j >= 0; j--)
                            {
                                Debug.Log(j);
                                AddOptionQuality[j] = (SlotItem as Item_Equipment).addOptionList[j].Quality;
                                AddOptionNum[j] = (SlotItem as Item_Equipment).addOptionList[j].Num;
                                AddOptionValue[j] = (SlotItem as Item_Equipment).addOptionList[j].value;
                            }
                        }
                        break;

                    case ItemInfo_Base.Kinds.Potion:
                        // ItemInfo_Consumable
                        DATA.I_StackData[i] = Slots[i].MyItems.Count;
                        // ItemInfo_Potion
                        break;
                }
            }
            else
            {
                // 만약 아이템이 없다면 없는 부분을 초기화해주는 부분
                DATA.I_Data[i] = null;
                DATA.I_ItemQuality[i] = 0;
                DATA.I_StackData[i] = 0;
            }
        }

        // 캐스팅 버튼 저장 부분
        for (int i = 0; i < 5; i++)
        {
            if (CastingButtons[i].Spell != null)
                DATA.ActionButtonsData[i] = (CastingButtons[i].Spell as Spell).ID;
            else
                DATA.ActionButtonsData[i] = null;
        }

        // 퀵슬롯 저장 부분
        for (int i = 0; i < 4; i++)
        {
            if (QuickSlotButtons[i].GetUseableItem != null)
                DATA.ActionButtonsData[i + 5] = (QuickSlotButtons[i].GetUseableItem.Peek() as Item_Base).ID;
            else
                DATA.ActionButtonsData[i + 5] = null;
        }

        // 지금까지의 변경사항을 저장한다.
        SaveLoadManager.DataSave(DATA, "Data");
    }

    public void LoadData()
    {
        if (SaveLoadManager.FileExists("Data"))
            DATA = SaveLoadManager.DataLoad<SaveLoadData>("Data");
        else
            DATA = new SaveLoadData();

        // Equipment 로드 부분
        for (int i = 0; i < 6; i++)
        {
            if (DATA.E_Data[i] != null)
            {
                if (DataTableManager.Instance.GetInfo_Equipment(DATA.E_Data[i]) != null)
                {
                    Item_Equipment DataItem = new Item_Equipment();
                    DataItem.SetInfo(DataTableManager.Instance.GetInfo_Equipment(DATA.E_Data[i]));
                    DataItem.Quality = (Item_Base.Qualitys)DATA.E_Quality[i];
                    for (int j = 0; j < (int)DataItem.Quality + 1; j++)
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
                    DATA.E_Data[i] = null;
                    DATA.E_Quality[i] = 0;
                }
            }
            else
            {
                DATA.E_Data[i] = null;
                DATA.E_Quality[i] = 0;
            }
        }

        // inventory 저장 부분
        for (int i = 0; i < 28; i++)
        {
            if (DATA.I_Data[i] != null)
            {
                switch (DATA.I_StackData[i])
                {
                    // 장비
                    case -1:
                        if (DataTableManager.Instance.GetInfo_Equipment(DATA.I_Data[i]) != null)
                        {
                            Item_Equipment DataItem = new Item_Equipment();
                            DataItem.SetInfo(DataTableManager.Instance.GetInfo_Equipment(DATA.I_Data[i]));
                            DataItem.Quality = (Item_Base.Qualitys)DATA.I_ItemQuality[i];
                            for (int j = 0; j < (int)DataItem.Quality + 1; j++)
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
                            DATA.I_Data[i] = null;
                            DATA.I_ItemQuality[i] = 0;
                            DATA.I_StackData[i] = 0;
                        }
                        break;

                    // 스택형 아이템
                    default:
                        if (DataTableManager.Instance.GetInfo_Consumable(DATA.I_Data[i]) != null)
                        {
                            string[] kind = DATA.I_Data[i].Split('_');
                            switch (kind[1])
                            {
                                case "Potion":
                                    Item_Potion DataItem = new Item_Potion();
                                    DataItem.SetInfo(DataTableManager.Instance.GetInfo_Consumable(DATA.I_Data[i]) as ItemInfo_Potion);
                                    for (int j = 0; j < DATA.I_StackData[i]; j++)
                                    {
                                        Slots[i].AddItem(DataItem);
                                        InventoryScript.MyInstance.OnItemCountChanged(DataItem);
                                    }
                                    break;

                                default:
                                    DATA.I_Data[i] = null;
                                    DATA.I_StackData[i] = 0;
                                    break;
                            }
                        }
                        else
                        {
                            DATA.I_Data[i] = null;
                            DATA.I_StackData[i] = 0;
                        }
                        break;
                }
            }
        }

        // ActionButton 저장 부분
        for (int i = 0; i < 5; i++)
        {
            if (DATA.ActionButtonsData[i] != null)
            {
                if (DataTableManager.Instance.GetInfo_Spell(DATA.ActionButtonsData[i]) != null)
                {
                    Spell DataSpell = new Spell();
                    DataSpell.SetSpellInfo(DataTableManager.Instance.GetInfo_Spell(DATA.ActionButtonsData[i]));
                    CastingButtons[i].SetUseable(DataSpell);
                }
                else
                    DATA.ActionButtonsData[i] = null;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (DATA.ActionButtonsData[i + 5] != null)
            {
                if (DataTableManager.Instance.GetInfo_Consumable(DATA.ActionButtonsData[i + 5]) != null)
                {
                    Item_Potion Dataitem = new Item_Potion();
                    Dataitem.SetInfo(DataTableManager.Instance.GetInfo_Consumable(DATA.ActionButtonsData[i + 5]) as ItemInfo_Potion);
                    QuickSlotButtons[i].SetUseable(Dataitem);
                }
                else
                    DATA.ActionButtonsData[i + 5] = null;
            }
        }
    }

    public void _LevelUp()
    {
        Player.MyInstance.MyStat.Level += 10;
    }

    public void _MoneyUp()
    {
        DATA.Gold += 10000;
    }

    public void _GetEquipment()
    {
        List<ItemInfo_Equipment> array = DataTableManager.Instance.GetInfo_Equipments(Player.MyInstance.MyStat.Level);

        for (int i =0; i < array.Count; i++)
        {
            ItemCart dropitem = Instantiate(Resources.Load<GameObject>("Prefabs/P_DropItem"),
                Player.MyInstance.transform.position + ((Vector3)Random.insideUnitCircle * 0.5f),
                Quaternion.identity).GetComponent<ItemCart>();

            dropitem.SetItem_Equipment(array[i], DataTableManager.Instance.GetQuality(Player.MyInstance.MyStat.Level));
        }
    }
}

[System.Serializable]
public class SaveLoadData
{
    // E == Equipment
    public string[] E_Data;
    public int[] E_Quality;
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

    // I == Inventory
    public string[] I_Data;
    public int[] I_ItemQuality;
    public int[] I_StackData;
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
        E_Data = new string[6];
        E_Quality = new int[6];
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

        I_Data = new string[28];
        I_ItemQuality = new int[28];
        I_StackData = new int[28];
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
