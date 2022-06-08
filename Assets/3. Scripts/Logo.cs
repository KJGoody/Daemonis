using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    Color color;
    [SerializeField]
    Image image;

    private void Start()
    {
        color = image.color;
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("ManagerScene");
        AsyncOperation asyncOper1 = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        asyncOper.allowSceneActivation = false;
        asyncOper1.allowSceneActivation = false;


        yield return new WaitForSeconds(3);
        while (color.a <= 1)
        {
            color.a += Time.deltaTime;
            image.color = color;
            yield return null;
        }
        asyncOper.allowSceneActivation = true;
        asyncOper1.allowSceneActivation = true;
    }
}
