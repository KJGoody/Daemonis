using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    private static TutorialPanel instance;
    public static TutorialPanel Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<TutorialPanel>();
            return instance;
        }
    }

    [SerializeField] private Tutorial Equip;
    [SerializeField] private Tutorial Spell;
    [SerializeField] private Tutorial Store;

    [HideInInspector] public bool Isdone;

    public IEnumerator StartTutorial(string tutorialName)
    {
        Isdone = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        switch (tutorialName)
        {
            case "Equip":
                Equip.StartTutorial();
                break;

            case "Spell":
                Spell.StartTutorial();
                break;

            case "Store":
                Store.StartTutorial();
                break;
        }
        while (true)
        {
            if (Isdone)
                break;
            yield return null;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
