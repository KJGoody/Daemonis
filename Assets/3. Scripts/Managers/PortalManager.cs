using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    void Start()
    {
        
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
