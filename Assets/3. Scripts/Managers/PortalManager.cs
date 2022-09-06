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

    [SerializeField]
    GameObject teleport_Panel;
    [SerializeField]
    GameObject returnPortal;
    [SerializeField]
    Button returnButton;
    private GameObject usingPortal;
    
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� �ε��ɶ� ����
    {
        ReturnPortalButton();
    }

    public void ReturnPortalButton()
    {
        Scene scene = SceneManager.GetSceneByName("Main");
        if(scene.name == "Main")
        {
            returnButton.interactable = false;
        }
        else
        {
            returnButton.interactable = true;
        }
    }

    public void ShowTeleportList(bool _bool)
    {
        teleport_Panel.SetActive(_bool);
    }

    public void _CreateReturnPortal()
    {
        if(usingPortal != null)
            Destroy(usingPortal);
    
        usingPortal = Instantiate(returnPortal, Player.MyInstance.transform.position,Quaternion.identity);

        switch (GameManager.MyInstance.CurrnetStageName)
        {
            case "1_Cave":
                usingPortal.GetComponent<ReturnPortal>().UnLoadSceneName = "1_Cave";
                usingPortal.GetComponent<ReturnPortal>().LoadSceneName = "Main";
                break;
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
        GameManager.MyInstance.CurrnetStageName = LoadSceneName;

        LoadingSceneManager.LoadScene(LoadSceneName);

        if (!Player.MyInstance.IsAlive)
        {
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
}
