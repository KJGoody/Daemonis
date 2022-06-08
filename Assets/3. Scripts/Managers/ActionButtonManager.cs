using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonManager : MonoBehaviour
{
    private static ActionButtonManager instance;
    public static ActionButtonManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ActionButtonManager>();
            return instance;
        }
    }

    public ActionButtonData SavedData { get; private set; }
    public ActionButtonData DATA;

    [SerializeField]
    private ActionButton[] SpellActionButton;
    [SerializeField]
    private ActionButton[] ItemActionButton;

    public void IsCoolDownOtherButton_Spell(IUseable useable)
    {
        foreach(ActionButton actionButton in SpellActionButton)
        {
            if(actionButton.MyUseable != null)
                if (actionButton.MyUseable.GetName().Equals(useable.GetName()))
                    Debug.Log(1111);
        }
    }

    public void IsCoolDownOtherButton_Item(ItemBase useable)
    {
        foreach (ActionButton actionButton in ItemActionButton)
        {
            if(actionButton.useables.Count != 0)
                if ((actionButton.useables.Peek() as ItemBase).MyName.Equals(useable.MyName))
                    Debug.Log(10);
        }
    }

    public void SaveData()
    {
        // 지금까지의 변경사항을 저장한다.
        SaveLoadManager.DataSave(DATA, "ActionButtonData");
    }

    public void LoadData()
    {
        if (SaveLoadManager.FileExists("ActionButtonData"))
            SavedData = SaveLoadManager.DataLoad<ActionButtonData>("ActionButtonData");
        else
            SavedData = new ActionButtonData(SpellActionButton, ItemActionButton);

        // 저장되어있는 사항을 저장한다.
        DATA = SavedData;
    }
}

public class ActionButtonData
{
    public ActionButton[] SpellActionButton;
    public ActionButton[] ItemActionButton;

    public ActionButtonData(ActionButton[] Spell, ActionButton[] Item)
    {

    }
}
