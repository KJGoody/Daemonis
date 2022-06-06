using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    private static OptionPanel instance;
    public static OptionPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<OptionPanel>();
            }

            return instance;
        }
    }
    public Toggle[] lootingQuality = new Toggle[6];
    public Slider volume_BGM;
    public Slider volume_SFX;
    public Toggle isMute_BGM;
    public Toggle isMute_SFX;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMute_BGM.isOn)
        {
            SoundManager.Instance.SetBGMMute(isMute_BGM.isOn);
        }
        else
        {
            SoundManager.Instance.SetBGMMute(isMute_BGM.isOn);
            SoundManager.Instance.SetBGMVolume(volume_BGM.value);
        }
        if (isMute_SFX.isOn)
        {
            SoundManager.Instance.SetSFXMute(isMute_SFX.isOn);
        }
        else
        {
            SoundManager.Instance.SetSFXMute(isMute_SFX.isOn);
            SoundManager.Instance.SetSFXVolume(volume_SFX.value);
        }
    }
}
