using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestPanel : MonoBehaviour
{
    #region Instance
    private static ChestPanel instance;
    public static ChestPanel Instance
    {
        get
        {
            if(instance == null)
                instance = FindObjectOfType<ChestPanel>();
            return instance;
        }
    }
    #endregion

    [SerializeField] private CanvasGroup chestPanael;
    [SerializeField] private CanvasGroup InventoryPanel;

    [SerializeField] private GameObject Joystick;

    [SerializeField] private GameObject EquipButton;
    [SerializeField] private GameObject PutButton;

    [SerializeField] private GameObject SelectItemPanel;


    //-- 슬롯 관리 --
    public Slot_Chest[] Slots;

    [SerializeField] private MoveToChestPanel MovePanel;

    //-- 선택 아이템 보여주기 --
    private Slot_Chest CurrentSlot;
    private Item_Base SelectItem;
    [SerializeField] private GameObject SelectPanel;
    [SerializeField] private Image SP_Image; // 선택한 아이템 화면에 보이는 이미지
    [SerializeField] private Text SP_Name;   // 아이템 이름
    [SerializeField] private Text SP_LimitLvl;// 제한 레벨
    [SerializeField] private Text SP_DefaultStat;// 기본효과(기본스탯같은) 설명
    [SerializeField] private Text SP_Descript;// 아이템 배경설명 (아이템 소개)
    [SerializeField] private Text SP_Quality;// 아이템 등급
    [SerializeField] private GameObject SP_Obj_Option;// 추가옵션 오브젝트
    [SerializeField] private GameObject[] SP_Obj_AddOptions;// 추가옵션들
    [SerializeField] private ContentSizeFitter SP_CSF_Descript;
    [SerializeField] private ContentSizeFitter SP_CSF_Panel;



    public void OpenChest()
    {
        chestPanael.alpha = 1;
        chestPanael.blocksRaycasts = true;

        if (InventoryPanel.alpha != 1)
        {
            UIManager.MyInstance.OpenClose(InventoryPanel);
        }

        Joystick.SetActive(false);
        EquipButton.SetActive(false);
        PutButton.SetActive(true);
    }

    public void CloseChest()
    {
        chestPanael.alpha = 0;
        chestPanael.blocksRaycasts = false;

        BuySellWindow.Instance._CloseWindow();

        Joystick.SetActive(true);
        EquipButton.SetActive(true);
        PutButton.SetActive(false);
    }

    //-- 슬롯 관리 --
    public void MoveToChest(Item_Base item)
    {
        if (item is Item_Consumable)
            MovePanel.SetMoveToChestPanel(item as Item_Consumable); 
        else
        {
            AddItem(item);
            item.Remove();
            HandScript.MyInstance.Close_SI_Panel();
            PlayerInfoPanel.Instance.Close_UE_Panel();
        }
    }

    private void AddItem(Item_Base itme, bool CansStack = false)
    {
        if (CansStack)
            if (PlaceInStack(itme as Item_Consumable))
                return;
        PlaceInEmpty(itme);
    }

    private bool PlaceInStack(Item_Consumable item)
    {
        foreach(Slot_Chest slot in Slots)
        {
            if (slot.StackItem(item))
            {
                return true;
            }
        }
        return false;
    }

    private void PlaceInEmpty(Item_Base item)
    {
        foreach(Slot_Chest slot in Slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public int GetEmptySlotNum()
    {
        int EmptyNum = 0;
        foreach (Slot_Chest slot in Slots)
            if (slot.IsEmpty)
                EmptyNum++;

        return EmptyNum;
    }

    // 상점에서 소모 아이템 구매 할때 구매 가능한 개수를 구하는 함수
    public int CanStackNum(Item_Base Item)
    {
        int CountNum = 0;
        foreach (Slot_Chest slot in Slots)
        {
            if (slot.IsEmpty)
                CountNum += (Item as Item_Consumable).StackSize;
            else
            {
                if (slot.Item.Name == Item.Name)
                    if (slot.GetItems.Count < (slot.Item as Item_Consumable).StackSize)
                        CountNum += (Item as Item_Consumable).StackSize - slot.GetItems.Count;
            }
        }
        return CountNum;
    }

    public void SelectItemEvent(Item_Base item)
    {
        SelectItem = item;
        SP_Image.sprite = item.Icon;
        SP_Name.text = item.Name;
        SP_Quality.text = item.QualityText;
        SP_LimitLvl.text = "제한 레벨 : " + item.LimitLevel;
        SP_DefaultStat.text = "사용 효과 : " + item.Effect;
        SP_Descript.text = item.Descript;
        switch (item.Kind)
        {
            case ItemInfo_Base.Kinds.Potion:
                SP_Obj_Option.SetActive(false);
                PlayerInfoPanel.Instance.Close_UE_Panel();
                break;

            case ItemInfo_Base.Kinds.Equipment: // 선택한 아이템이 장비일 때 추옵, 세트옵 표시
                SP_Obj_Option.SetActive(true);

                for (int i = 0; i < (item as Item_Equipment).addOptionList.Count; i++) // 추옵 표시
                {
                    ItemAddOptionInfo optionInfo = SP_Obj_AddOptions[i].GetComponent<ItemAddOptionInfo>();
                    optionInfo.SetAddOptionPrefab((item as Item_Equipment).addOptionList[i]);
                    SP_Obj_AddOptions[i].SetActive(true);
                }
                for (int i = 6; i > (item as Item_Equipment).addOptionList.Count; i--)
                {
                    SP_Obj_AddOptions[i - 1].SetActive(false);
                }

                // 장비 부위에 따라 착용중인 장비 표시
                int partNum = (int)(item as Item_Equipment).Part;
                if (Player.MyInstance.usingEquipment[partNum] != null)
                {
                    PlayerInfoPanel.Instance.ShowUsingEquipment(partNum);
                }
                else
                {
                    PlayerInfoPanel.Instance.Close_UE_Panel();
                }
                break;
        }
        SelectPanel.SetActive(true);

        // content size filtter 바로 안늘어나는 버그 해결용
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)SP_CSF_Descript.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)SP_CSF_Panel.transform);
    }

    public void _TackOut()
    {
        if(SelectItem is Item_Consumable)
            MovePanel.SetMoveToChestPanel(SelectItem as Item_Consumable);
        else
        {
            InventoryScript.MyInstance.AddItem(SelectItem);
            SelectItem.Remove();
            SelectPanel.SetActive(false);
            PlayerInfoPanel.Instance.Close_UE_Panel();
        }
    }

    public void _Close()
    {
        SelectPanel.SetActive(false);
    }

    public void _Remove()
    {
        SelectItem.Remove();
        SelectPanel.SetActive(false);
    }
}
