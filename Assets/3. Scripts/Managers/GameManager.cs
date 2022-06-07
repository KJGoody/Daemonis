using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // 페이드인 효과
    Color color;
    [SerializeField]
    Image fadeIn_IMG;
    [SerializeField]
    GameObject fadeIn_OBJ;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private int gold;
    public int MyGold
    {
        get
        {
            return gold;
        }
        set
        {
            if (value <= 0)
                value = 0;
            gold = value;
        }
    }
    
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject quitPanel;

    [SerializeField]
    private GameObject[] dontDestroyObj;
    private NPC currentTarget;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //for(int i = 0; i < dontDestroyObj.Length; i++)
        //{
        //    DontDestroyOnLoad(dontDestroyObj[i]);
        //}
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!quitPanel.activeSelf)
                quitPanel.SetActive(true);
            else
                quitPanel.SetActive(false);
        }
        ClickTarget();
    }
    
    private void ClickTarget()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));
            if (hit.collider != null)
            {
                if (currentTarget != null)
                    currentTarget.DeSelect();
                currentTarget = hit.collider.GetComponent<NPC>();
                player.MyTarget = currentTarget.Select();
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }

                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 로딩될때 실행
    {
        Debug.Log("a");

            Debug.Log("b");
            StartCoroutine(FadeIn());


    }
    public IEnumerator FadeIn()
    {
        Debug.Log("c");
        fadeIn_OBJ.SetActive(true);
        Debug.Log("d");
        while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            fadeIn_IMG.color = color;
            yield return null;
        }
        Debug.Log("e");
        fadeIn_OBJ.SetActive(false);
        color.a = 1;
        fadeIn_IMG.color = color;
        Debug.Log("f");

    }
    public void GameQuit()
    {
        Application.Quit();
    }
    public void GoLobby()
    {
        if(SceneManager.GetSceneByName("1_Cave").IsValid())
        {
            Debug.Log("asdf");
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
        //SceneManager.LoadScene("1_Cave", LoadSceneMode.Additive);

    }
}