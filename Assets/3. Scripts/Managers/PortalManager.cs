using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    private static PortalManager instance;
    public static PortalManager MyInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PortalManager>();
            return instance;
        }
    }

    [SerializeField] GameObject teleport_Panel;
    [SerializeField] GameObject returnPortal;
    [SerializeField] Button returnButton;
    private GameObject usingPortal;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 로딩될때 실행
    {
        ReturnPortalButton();
    }

    public void ReturnPortalButton()
    {
        Scene scene = SceneManager.GetSceneByName("3.Lobby");
        if (scene.name == "3.Lobby")
            returnButton.interactable = false;
        else
            returnButton.interactable = true;
    }

    public void ShowTeleportList(bool _bool)
    {
        teleport_Panel.SetActive(_bool);
    }

    public void _CreateReturnPortal()
    {
        if (usingPortal != null)
            Destroy(usingPortal);

        usingPortal = Instantiate(returnPortal, Player.MyInstance.transform.position, Quaternion.identity);

        if (GameManager.MyInstance.CurrnetSceneName == "5.IngameMap")
        {
            usingPortal.GetComponent<ReturnPortal>().UnLoadSceneName = "5.IngameMap";
            usingPortal.GetComponent<ReturnPortal>().LoadSceneName = "3.Lobby";
        }
    }

    public void _UnloadSceneName(string UnloeadSceneName)
    {
        if (SceneManager.GetSceneByName(UnloeadSceneName).IsValid())
        {
            SceneManager.UnloadSceneAsync(UnloeadSceneName);
            GameManager.MyInstance.CallUnLoadSceneEvent();
        }
    }

    public void _LoadSceneName(string LoadSceneName)
    {
        GameManager.MyInstance.SaveData();
        GameManager.MyInstance.CurrnetSceneName = LoadSceneName;

        LoadingSceneManager.LoadScene(LoadSceneName);

        // 플레이어 되살아날때에 처리
        if (!Player.MyInstance.IsAlive)
        {
            Player.MyInstance.IsAlive = true;
            Player.MyInstance.transform.Find("HitBox_Player").gameObject.SetActive(true);
            Player.MyInstance.rigid2D.simulated = true;
            Player.MyInstance.NewBuff("Skill_Fire_02");
        }
        Player.MyInstance.MyStat.CurrentHealth = Player.MyInstance.MyStat.CurrentMaxHealth;
        Player.MyInstance.MyStat.CurrentMana = Player.MyInstance.MyStat.CurrentMaxMana;

        BossHPBar.Instance.BossHPBarSetActive(false);
        if (usingPortal != null)
            Destroy(usingPortal);
    }

    public void _SetCurrentStageID(string StageID)
    {
        GameManager.MyInstance.CurrentStageID = StageID;
    }
}
