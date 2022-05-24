using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    public Image[] equipment_Img = new Image[6];
    void Start()
    {
        Player.MyInstance.useEquipment += ChangeEquipment;
    }

    public void initEquipment()
    {
        for(int i = 0; i < equipment_Img.Length; i++)
        {
            ChangeEquipment(i);
        }
    }

    public void ChangeEquipment(int partNum)
    {
        if (Player.MyInstance.usingEquipment[partNum] != null)
        {
            equipment_Img[partNum].sprite = Player.MyInstance.usingEquipment[partNum].MyIcon;
            equipment_Img[partNum].color= new Color(1,1,1,1);
        }
    }
    void Update()
    {
        
    }
}
