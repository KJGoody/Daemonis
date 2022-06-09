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
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public SaveLoadData SavedData { get; private set; }
    public SaveLoadData DATA;

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject quitPanel;

    [SerializeField]
    private GameObject[] dontDestroyObj;
    private NPC currentTarget;

    private void Awake()
    {
        LoadData();
        //for (int i = 0; i < dontDestroyObj.Length; i++)
        //{
        //    DontDestroyOnLoad(dontDestroyObj[i]);
        //}
        SceneManager.sceneLoaded += OnSceneLoaded;
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
                    currentTarget.DeSelect();
                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 로딩될때 실행
    {
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeIn()
    {
        fadeIn_OBJ.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            fadeIn_IMG.color = color;
            yield return null;
        }

        if(fadeIn_IMG.color.a <=0)
            fadeIn_OBJ.SetActive(false);

        color.a = 1;
        fadeIn_IMG.color = color;

    }
    public void GameQuit()
    {
        Application.Quit();
    }
    public void GoLobby()
    {
        if (SceneManager.GetSceneByName("1_Cave").IsValid())
        {
            Debug.Log("asdf");
            SceneManager.UnloadSceneAsync("1_Cave");
        }
        Player.MyInstance.MyStat.CurrentHealth = Player.MyInstance.MyStat.CurrentMaxHealth;
        Player.MyInstance.MyStat.CurrentMana = Player.MyInstance.MyStat.CurrentMaxMana;
        Player.MyInstance.rigid2D.simulated = true;
        Player.MyInstance.transform.Find("HitBox_Player").gameObject.SetActive(true);
        Player.MyInstance.NewBuff("Skill_Fire_02_Buff");
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
    public void RestartCave()
    {
        if (SceneManager.GetSceneByName("1_Cave").IsValid())
        {
            SceneManager.UnloadSceneAsync("1_Cave");
        }
        Player.MyInstance.MyStat.CurrentHealth = Player.MyInstance.MyStat.CurrentMaxHealth;
        Player.MyInstance.MyStat.CurrentMana = Player.MyInstance.MyStat.CurrentMaxMana;
        Player.MyInstance.rigid2D.simulated = true;
        Player.MyInstance.transform.Find("HitBox_Player").gameObject.SetActive(true);
        Player.MyInstance.NewBuff("Skill_Fire_02_Buff");
        LoadingSceneManager.LoadScene("1_Cave");
        //SceneManager.LoadScene("1_Cave", LoadSceneMode.Additive);
    }

    public void SaveData()
    {
        // 지금까지의 변경사항을 저장한다.
        SaveLoadManager.DataSave(DATA, "Data");
    }

    public void LoadData()
    {
        if (SaveLoadManager.FileExists("Data"))
            SavedData = SaveLoadManager.DataLoad<SaveLoadData>("Data");
        else
            SavedData = new SaveLoadData();

        // 저장되어있는 사항을 저장한다.
        DATA = SavedData;
    }
}

[System.Serializable]
public class SaveLoadData
{
    public int Gold;

    public SaveLoadData()
    {
        Gold = 0;
    }
}
