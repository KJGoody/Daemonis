using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    Color color;
    [SerializeField] Image BlackBackImage;

    private void Start()
    {
        color = BlackBackImage.color;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // 비동기로 씬을 불러온다.
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("2.ManagerScene");
        AsyncOperation asyncOper1 = SceneManager.LoadSceneAsync("3.Lobby", LoadSceneMode.Additive);
        asyncOper.allowSceneActivation = false;
        asyncOper1.allowSceneActivation = false;

        yield return new WaitForSeconds(2);
        while (color.a <= 1)
        {
            color.a += Time.deltaTime;
            BlackBackImage.color = color;
            yield return null;
        }

        asyncOper.allowSceneActivation = true;
        asyncOper1.allowSceneActivation = true;
    }
}
