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
    private string NPCName;
    [SerializeField] private Text ActorName;
    [SerializeField] private Text ActorSpeech;

    [SerializeField] private GameObject Joystick;

    private bool IsSkip = false;

    public void OpenDialog(string NPCname, int inedex = 0)
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        NPCName = NPCname;
        Joystick.SetActive(false);
        StartCoroutine(Dialog(inedex));
    }

    public IEnumerator Dialog(int index)
    {
        List<DialogData> data;
        if (index != 0)
            data = DataTableManager.Instance.GetDialogArray(index);
        else
            data = DataTableManager.Instance.GetDialogArray();

        for (int i = 0; i < data.Count; i++)
        {
            GetComponent<CanvasGroup>().alpha = 1;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            yield return StartCoroutine(Acting(data[i].ActorName, data[i].Speech));
        }

        QuestPanel.Instance.TalkDone(NPCName);
        yield return new WaitForSeconds(0.1f);

        Joystick.SetActive(true);
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        switch (NPCName)
        {
            case "Merchant":
                ActiveButton.Instance.SetButton(ActiveButton.Role.MerchantButton, Merchant.IsQuestTalk);
                break;

            case "Quester":
                ActiveButton.Instance.SetButton(ActiveButton.Role.QuesterButton, Quester.IsQuestTalk);
                break;
        }
    }

    private IEnumerator Acting(string actorName, string actorSpeech)
    {
        switch (actorName)
        {
            case "Tutorial":
                GetComponent<CanvasGroup>().alpha = 0;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                yield return StartCoroutine(TutorialPanel.Instance.StartTutorial(actorSpeech));
                break;

            default:
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
                break;
        }
    }

    public void _ClickSkipButton()
    {
        IsSkip = true;
    }
}
