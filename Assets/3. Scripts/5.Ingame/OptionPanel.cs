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
    public Slider volume_BGM; // 배경음악 볼륨
    public Slider volume_SFX; // 효과음 볼륨
    public Toggle isMute_BGM; // 배경음악 음소거
    public Toggle isMute_SFX; // 효과음 음소거
    public Text text_BGM; // 배경음악 음량표시
    public Text text_SFX; // 효과음 음량표시

    public void BGMSlider() // 배경음량 설정
    {
        SoundManager.Instance.SetBGMVolume(volume_BGM.value);
        text_BGM.text = "" + (int)(volume_BGM.value * 100);
    }
    public void SFXSlider() // 효과음량 설정
    {
        SoundManager.Instance.SetSFXVolume(volume_SFX.value);
        text_SFX.text = "" + (int)(volume_SFX.value * 100);
    }
    public void BGMToggle() // 배경 음소거
    {
        SoundManager.Instance.SetBGMMute(isMute_BGM.isOn);
    }
    public void SFXToggle() // 효과 음소거
    {
        SoundManager.Instance.SetSFXMute(isMute_SFX.isOn);
    }

}
