using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPanel : MonoBehaviour
{
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

    [SerializeField] private CanvasGroup chestPanael;
    [SerializeField] private CanvasGroup InventoryPanel;

    [SerializeField] private GameObject Joystick;

    [SerializeField] private GameObject EquipButton;
    [SerializeField] private GameObject PutButton;

    [SerializeField] private GameObject SelectItem;

    [SerializeField] private SlotScript[] ChestSlots;


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
}
