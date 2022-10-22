using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    private Image img;
    [SerializeField]
    private Slider slider;
    private float time = 0;

    void Start()
    {
        StartCoroutine(LoadScene());
        InitScene();
    }

    public static void LoadScene(string sceneNmae)
    {
        nextScene = sceneNmae;
        SceneManager.LoadSceneAsync("4.LoadingScene", LoadSceneMode.Additive);
    }

    private void InitScene() // æ¿ ±≥√º Ω√ √ ±‚»≠
    {
        Player.MyInstance.transform.position = new Vector3(0, 0, 0);
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i <enemys.Length; i++)
        {
            Destroy(enemys[i]);
        }
        GameObject[] dropItems = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < dropItems.Length; i++)
        {
            Destroy(dropItems[i]);
        }
        GameObject[] skills = GameObject.FindGameObjectsWithTag("Skill");
        for (int i = 0; i < skills.Length; i++)
        {
            Destroy(skills[i]);
        }
    }

    private IEnumerator LoadScene() // æ¿ ∑Œµ˘
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        time = 0;
        while (!op.isDone)
        {

            time += Time.deltaTime;

            slider.value = time / 2.5f;

            if (time > 2.5f)
            {
                op.allowSceneActivation = true;
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("4.LoadingScene"));
                GameManager.MyInstance.StartCoroutine(GameManager.MyInstance.FadeIn());
            }

            yield return null;
        }
    }
}
