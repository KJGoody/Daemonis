using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chester : NPC
{
    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);

        if (target != null)
        {
            ActiveButton.Instance.SetButton(ActiveButton.Role.ChesterButton);
        }
        else
        {
            ActiveButton.Instance.ResetButton();
        }
    }

    public override Transform Select()
    {
        ChestPanel.Instance.OpenChest();
        return base.Select();
    }
}
