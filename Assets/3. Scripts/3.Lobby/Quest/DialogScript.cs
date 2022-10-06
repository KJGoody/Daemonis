using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogScript : MonoBehaviour
{
    private static DialogScript instance;
    public static DialogScript Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DialogScript>();
            return instance;
        }
    }
    [SerializeField] private Text ActorName;
    [SerializeField] private Text ActorSpeech;

    private bool IsSkip = false;

    public void OpenQuestPanel()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        StartCoroutine(Dialog());
    }

    public IEnumerator Dialog()
    {
        List<DialogData> data = DataTableManager.Instance.GetDialogArray();
        for(int i = 0; i < data.Count; i++)
            yield return StartCoroutine(Acting(data[i].ActorName, data[i].Speech));

        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        ActiveButton.Instance.SetButton(ActiveButton.Role.QuesterButton);
    }

    private IEnumerator Acting(string actorName, string actorSpeech)
    {
        ActorName.text = actorName;

        string writerText = "";
        for (int i = 0; i < actorSpeech.Length; i++)
        {
            if (IsSkip)
            {
                ActorSpeech.text = actorSpeech;
                IsSkip = false;
                break;
            }

            writerText += actorSpeech[i];
            ActorSpeech.text = writerText;
            yield return new WaitForSeconds(0.05f);
        }

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
                break;
            yield return null;
        }

        IsSkip = false;
    }

    public void _ClickSkipButton()
    {
        IsSkip = true;
    }
}
