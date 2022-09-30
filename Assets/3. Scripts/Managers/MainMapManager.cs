using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapManager : MonoBehaviour
{
    public Text actName_Text;
    public GameObject[] actList_Obj;
    private int selectActNum;

    private void Start()
    {
        selectActNum = 0;
        SetActInfo(selectActNum);
    }

    public void MoveButton(bool next)
    {
        if (next)
        {
            if (selectActNum == actList_Obj.Length - 1)
                selectActNum = 0;
            else
                selectActNum++;
        }
        else
        {
            if (selectActNum == 0)
                selectActNum = actList_Obj.Length - 1;
            else
                selectActNum--;
        }
        SetActInfo(selectActNum);
    }

    private void SetActInfo(int num)
    {
        switch (num)
        {
            case 0:
                actName_Text.text = "Act_1";
                break;
            case 1:
                actName_Text.text = "Act_2";
                break;
        }
        for(int i = 0; i < actList_Obj.Length; i++)
        {
            if (i == num)
                actList_Obj[i].SetActive(true);
            else
                actList_Obj[i].SetActive(false);
        }
    }

}
