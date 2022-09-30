using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockStageButton : MonoBehaviour
{
    [SerializeField] private Button MyButton;
    [SerializeField] private string Stage;

    private void OnEnable()
    {
        MyButton.interactable = false;

        string[] stringSplit = Stage.Split('_');
        if (GameManager.MyInstance.DATA.ClearStageNum[int.Parse(stringSplit[1]) - 1] != 0)
            if (GameManager.MyInstance.DATA.ClearStageNum[int.Parse(stringSplit[1]) - 1] + 1 >= int.Parse(stringSplit[2]))
            {
                MyButton.interactable = true;
                gameObject.SetActive(false);
            }

    }
}
