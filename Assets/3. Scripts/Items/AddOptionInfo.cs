using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOptionInfo : MonoBehaviour
{
    AddOption addOption;

    [SerializeField] private Sprite[] tierImage;
    public Image tierIcon;
    public string option_Name;
    public Text optionText;
    public void SetAddOptionPrefab(AddOption option)
    {
        addOption = option;
        option_Name = AddOptionManager.MyInstance.GetOptionName(addOption.option_Num);
        SetOption();
        
        
    }
    public void SetOption() // �̹����� �ؽ�Ʈ ����
    {
        tierIcon.sprite = tierImage[addOption.tier];
        optionText.text = option_Name + " " + addOption.value.ToString("F2");
    }
}
