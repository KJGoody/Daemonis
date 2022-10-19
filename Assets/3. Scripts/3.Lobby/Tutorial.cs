using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] Scenes;

    public void StartTutorial()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        for(int i = 0; i < Scenes.Length; i++)
        {
            yield return StartCoroutine(PlayScene(i));
        }
        TutorialPanel.Instance.Isdone = true;
    }

    private IEnumerator PlayScene(int index)
    {
        Scenes[index].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
                break;
            yield return null;
        }
        Scenes[index].SetActive(false);
    }
}
