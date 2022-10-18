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

    private void Start()
    {
        StartTutorial_Equipment();
    }

    public void StartTutorial_Equipment()
    {
        Equip.StartTutorial();
    }

    public void StartTutorial_Spell()
    {
        Equip.StartTutorial();
    }

    public void StartTutorial_Store()
    {
        Equip.StartTutorial();
    }

    public void EndTutorial(Tutorial.TutorialNames tutorialName)
    {
        switch (tutorialName)
        {
            case Tutorial.TutorialNames.Equip:
                break;

            case Tutorial.TutorialNames.Spell:
                break;

            case Tutorial.TutorialNames.Store:
                break;
        }
    }
}
