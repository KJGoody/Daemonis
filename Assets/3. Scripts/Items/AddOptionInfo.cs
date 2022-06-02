using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOptionInfo : MonoBehaviour
{
    public int tier;
    public Image tierIcon;
    public string option;
    public float value;
    public Text optionText;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void SetOption()
    {
        optionText.text = option + " " + value;
    }
}
