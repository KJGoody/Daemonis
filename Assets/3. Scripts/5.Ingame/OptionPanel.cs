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
    public Slider volume_BGM; // ������� ����
    public Slider volume_SFX; // ȿ���� ����
    public Toggle isMute_BGM; // ������� ���Ұ�
    public Toggle isMute_SFX; // ȿ���� ���Ұ�
    public Text text_BGM; // ������� ����ǥ��
    public Text text_SFX; // ȿ���� ����ǥ��

    public void BGMSlider() // ������� ����
    {
        SoundManager.Instance.SetBGMVolume(volume_BGM.value);
        text_BGM.text = "" + (int)(volume_BGM.value * 100);
    }
    public void SFXSlider() // ȿ������ ����
    {
        SoundManager.Instance.SetSFXVolume(volume_SFX.value);
        text_SFX.text = "" + (int)(volume_SFX.value * 100);
    }
    public void BGMToggle() // ��� ���Ұ�
    {
        SoundManager.Instance.SetBGMMute(isMute_BGM.isOn);
    }
    public void SFXToggle() // ȿ�� ���Ұ�
    {
        SoundManager.Instance.SetSFXMute(isMute_SFX.isOn);
    }

}
