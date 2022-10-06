using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quester : NPC
{
    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);

        if (target != null)
            ActiveButton.Instance.SetButton(ActiveButton.Role.QuesterButton);
        else
            ActiveButton.Instance.ResetButton();
    }
}
