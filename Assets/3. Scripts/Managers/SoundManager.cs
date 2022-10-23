using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//이 게임에서는 거리에 비례해 사운드의 크기를 조절할 필요가 없기에 하나의 AudioSource로 AudioClip들을 돌려가며 실행시킬 것이다.
//배경음악을 실행할 AudioSource와 효과음을 실행할 AudioSource를 SoundManager의 자식 오브젝트로 설정
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SoundManager>();
            return instance;
        }
    } // Sound를 관리해주는 스크립트는 하나만 존재해야하고 instance프로퍼티로 언제 어디에서나 불러오기위해 싱글톤 사용

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeSFX = 1f;
    public float masterVolumeBGM = 1f;
    [SerializeField]
    private AudioClip lobbyBgmAudioClip; // 로비에서 사용할 BGM
    [SerializeField]
    private AudioClip caveBgmAudioClip; // 액트1 동굴에서 사용할 BGM
    //[SerializeField]
    //private AudioClip loadingBgmAudioClip; // 로딩씬에서 사용할 BGM
    //[SerializeField]
    //private AudioClip nightBgmAudioClip; // 저녁맵에서 사용할 BGM
    //[SerializeField]
    //private AudioClip chaosBgmAudioClip; // 카오스맵에서 사용할 BGM
    //[SerializeField]
    //private AudioClip BossBgmAudioClip; // 카오스맵에서 사용할 BGM
    //[SerializeField]
    //private AudioClip CreditAudioClip; // 크레딧창에서 사용할 BGM

    [SerializeField]
    private AudioClip[] sfxAudioClips; //효과음들 지정

    Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>(); //효과음 딕셔너리
    // AudioClip을 Key,Value 형태로 관리하기 위해 딕셔너리 사용

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(this.gameObject); //여러 씬에서 사용할 것.

        SceneManager.sceneLoaded += OnSceneLoaded;

        bgmPlayer = GameObject.Find("BGMSoundPlayer").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("SFXSoundPlayer").GetComponent<AudioSource>();

        foreach (AudioClip audioclip in sfxAudioClips)
        {
            audioClipsDic.Add(audioclip.name, audioclip);
        }

        if (!PlayerPrefs.HasKey("BGMVolume"))
            PlayerPrefs.SetFloat("BGMVolume", 1f);

        if (!PlayerPrefs.HasKey("SFXVolume"))
            PlayerPrefs.SetFloat("SFXVolume", 1f);

        if (!PlayerPrefs.HasKey("BGMMute"))
            PlayerPrefs.SetInt("BGMMute", System.Convert.ToInt16(false));

        if (!PlayerPrefs.HasKey("SFXMute"))
            PlayerPrefs.SetInt("SFXMute", System.Convert.ToInt16(false));

        SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume"));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
        bgmPlayer.mute = System.Convert.ToBoolean(PlayerPrefs.GetInt("BGMMute"));
        sfxPlayer.mute = System.Convert.ToBoolean(PlayerPrefs.GetInt("SFXMute"));
        masterVolumeBGM = PlayerPrefs.GetFloat("BGMVolume");
        masterVolumeSFX = PlayerPrefs.GetFloat("SFXVolume");
    }

    // 효과 사운드 재생 : 이름을 필수 매개변수, 볼륨을 선택적 매개변수로 지정
    public void PlaySFXSound(string name, float volume = 1f)
    {
        if(name != "None")
        {
            if (audioClipsDic.ContainsKey(name) == false)
            {
                Debug.Log(name + " is not Contained audioClipsDic");
                return;
            }
            sfxPlayer.PlayOneShot(audioClipsDic[name], volume * masterVolumeSFX);
        }
    }

    public void PlaySFXSound(AudioSource parent, string name, float volume = 1f)
    {
        if (audioClipsDic.ContainsKey(name) == false)
            return;
        parent.PlayOneShot(audioClipsDic[name], volume * masterVolumeSFX);
    }

    public void PlaySFXSoundLoop(string name, Transform parent, float volume = 1f)
    {
        if (audioClipsDic.ContainsKey(name) == false)
            return;
        
        AudioSource audioSource = Instantiate(Resources.Load("SFXSoundPlayer") as GameObject, parent).GetComponent<AudioSource>();
        audioSource.loop = true; 
        audioSource.clip = audioClipsDic[name];
        audioSource.volume = volume * masterVolumeSFX;
        audioSource.Play();
    }

    //BGM 사운드 재생 : 볼륨을 선택적 매개변수로 지정
    public void PlayBGMSound(Scene scene ,float volume = 1f)
    {
        bgmPlayer.loop = true; //BGM 사운드이므로 루프설정
        //bgmPlayer.volume = volume * masterVolumeBGM;
        if (scene.name == "3.Lobby")
        {
            bgmPlayer.clip = lobbyBgmAudioClip;
            bgmPlayer.Play();
        }
        else if (scene.name == "5.IngameMap")
        {
            bgmPlayer.clip = caveBgmAudioClip;
            bgmPlayer.Play();
        }
        //else if (SceneManager.GetActiveScene().name == "LoadingScene")
        //{
        //    bgmPlayer.clip = loadingBgmAudioClip;
        //    bgmPlayer.Play();
        //}
        //현재 씬에 맞는 BGM 재생
    }
    public void PlayCoustomBGM(string name)
    {
        AudioClip playClip = null;
        //switch (name)
        //{
        //    case "Boss":
        //        playClip = BossBgmAudioClip;
        //        break;
        //    case "NightStage":
        //        playClip = nightBgmAudioClip;
        //        break;
        //    case "ChaosStage":
        //        playClip = chaosBgmAudioClip;
        //        break;
        //    case "Credit":
        //        playClip = CreditAudioClip;
        //        break;
        //    case "Menu":
        //        playClip = menuBgmAudioClip;
        //        break;
        //}
        bgmPlayer.clip = playClip;
        bgmPlayer.Play();
    }
    public void SetBGMVolume(float volume)
    {
        bgmPlayer.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        sfxPlayer.volume = volume;
    }
    public void SetBGMMute(bool mute)
    {
        bgmPlayer.mute = mute;
    }
    public void SetSFXMute(bool mute)
    {
        sfxPlayer.mute = mute;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 로딩될때 실행
    {
        PlayBGMSound(scene);
    }

    public void PlayUISound()
    {
        if (audioClipsDic.ContainsKey("ClickSound") == false)
        {
            Debug.Log("ClickSound" + " is not Contained audioClipsDic");
            return;
        }
        sfxPlayer.PlayOneShot(audioClipsDic["ClickSound"], masterVolumeSFX);
    }
}