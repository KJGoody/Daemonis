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


    //-- ���� ���� --
    public Slot_Chest[] Slots;

    [SerializeField] private MoveToChestPanel MovePanel;

    //-- ���� ������ �����ֱ� --
    private Slot_Chest CurrentSlot;
    private Item_Base SelectItem;
    [SerializeField] private GameObject SelectPanel;
    [SerializeField] private Image SP_Image; // ������ ������ ȭ�鿡 ���̴� �̹���
    [SerializeField] private Text SP_Name;   // ������ �̸�
    [SerializeField] private Text SP_LimitLvl;// ���� ����
    [SerializeField] private Text SP_DefaultStat;// �⺻ȿ��(�⺻���Ȱ���) ����
    [SerializeField] private Text SP_Descript;// ������ ��漳�� (������ �Ұ�)
    [SerializeField] private Text SP_Quality;// ������ ���
    [SerializeField] private GameObject SP_Obj_Option;// �߰��ɼ� ������Ʈ
    [SerializeField] private GameObject[] SP_Obj_AddOptions;// �߰��ɼǵ�
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

    //-- ���� ���� --
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

    // �������� �Ҹ� ������ ���� �Ҷ� ���� ������ ������ ���ϴ� �Լ�
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
        SP_LimitLvl.text = "���� ���� : " + item.LimitLevel;
        SP_DefaultStat.text = "��� ȿ�� : " + item.Effect;
        SP_Descript.text = item.Descript;
        switch (item.Kind)
        {
            case ItemInfo_Base.Kinds.Potion:
                SP_Obj_Option.SetActive(false);
                PlayerInfoPanel.Instance.Close_UE_Panel();
                break;

            case ItemInfo_Base.Kinds.Equipment: // ������ �������� ����� �� �߿�, ��Ʈ�� ǥ��
                SP_Obj_Option.SetActive(true);

                for (int i = 0; i < (item as Item_Equipment).addOptionList.Count; i++) // �߿� ǥ��
                {
                    ItemAddOptionInfo optionInfo = SP_Obj_AddOptions[i].GetComponent<ItemAddOptionInfo>();
                    optionInfo.SetAddOptionPrefab((item as Item_Equipment).addOptionList[i]);
                    SP_Obj_AddOptions[i].SetActive(true);
                }
                for (int i = 6; i > (item as Item_Equipment).addOptionList.Count; i--)
                {
                    SP_Obj_AddOptions[i - 1].SetActive(false);
                }

                // ��� ������ ���� �������� ��� ǥ��
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

        // content size filtter �ٷ� �ȴþ�� ���� �ذ��
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
