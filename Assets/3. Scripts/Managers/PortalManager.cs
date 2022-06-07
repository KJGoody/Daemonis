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
            {
                instance = FindObjectOfType<PortalManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    GameObject teleport_Panel;
    [SerializeField]
    GameObject returnPortal;
    [SerializeField]
    Button returnButton;
    [SerializeField]
    private GameObject usingPortal;
    
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void CreateReturnPortal()
    {
        if(usingPortal != null)
        {
            Destroy(usingPortal);
        }
        usingPortal = Instantiate(returnPortal, Player.MyInstance.transform.position,Quaternion.identity);

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� �ε��ɶ� ����
    {
        aSDF();
    }
    public void aSDF()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        
        Scene scene = SceneManager.GetSceneByName("Main");
        Debug.Log(scene.name);
        if(scene.name == "Main")
        {
            returnButton.interactable = false;
        }
        else
        {
            Debug.Log("asdfasdfasdf");
            returnButton.interactable = true;
        }
    }

    public void ShowTeleportList()
    {
        teleport_Panel.SetActive(true);
    }
    public void GoLobby()
    {
        if (SceneManager.GetSceneByName("1_Cave").IsValid())
        {
            SceneManager.UnloadSceneAsync("1_Cave");
        }
        LoadingSceneManager.LoadScene("Main");
        Destroy(usingPortal);
    }

    public void GoCave()
    {
        if (SceneManager.GetSceneByName("Main").IsValid())
        {
            SceneManager.UnloadSceneAsync("Main");
        }
        LoadingSceneManager.LoadScene("1_Cave");
    }
}
