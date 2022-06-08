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
}
