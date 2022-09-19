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

    [SerializeField] private GameObject Joystick;

    [SerializeField] private GameObject EquipButton;
    [SerializeField] private GameObject PutButton;

    [SerializeField] private GameObject SelectItem;


    public void OpenChest()
    {
        chestPanael.alpha = 1;
        chestPanael.blocksRaycasts = true;

        Joystick.SetActive(false);
        EquipButton.SetActive(false);
        PutButton.SetActive(true);
    }

    public void CloseChest()
    {
        chestPanael.alpha = 0;
        chestPanael.blocksRaycasts = false;
    }
}
