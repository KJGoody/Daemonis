using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // ���̵��� ȿ��
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // �������� ����
        {
            if (!quitPanel.activeSelf)
                quitPanel.SetActive(true);
            else
                quitPanel.SetActive(false);
        }
        ClickTarget();
    }

    private void ClickTarget() // Ÿ�� ����
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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� �ε��ɶ� ����
    {
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeIn() // ������ ���̵���
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
    public void GameQuit() //���� ����
    {
        Application.Quit();
    }


    public void SaveData()
    {
        // ���ݱ����� ��������� �����Ѵ�.
        SaveLoadManager.DataSave(DATA, "Data");
    }

    public void LoadData()
    {
        if (SaveLoadManager.FileExists("Data"))
            SavedData = SaveLoadManager.DataLoad<SaveLoadData>("Data");
        else
            SavedData = new SaveLoadData();

        // ����Ǿ��ִ� ������ �����Ѵ�.
        DATA = SavedData;
        DATA.LoadData();
        Debug.Log(DATA.ActionButtonIUseable);
    }
}

[System.Serializable]
public class SaveLoadData
{
    public int Gold;

    // �׼ǹ�ư �ν��Ͻ�
    public ActionButton[] ActionButtons;
    public IUseable[] ActionButtonIUseable = new IUseable[9];


    public SaveLoadData()
    {
        Gold = 0;
    }

    public void LoadData()
    {
        // �����Ǿ� ���� ���� �׼ǹ�ư �ν��Ͻ��� ����Ǿ� �ִ� IUseable�� �����Ѵ�.
        for(int i = 0; i < 9; i++)
            if(ActionButtonIUseable[i] != null)
                ActionButtons[i].SetUseable(ActionButtonIUseable[i]);
    }
}
