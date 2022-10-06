using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : NPC
{
    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);

        if(target != null)
            ActiveButton.Instance.SetButton(ActiveButton.Role.MerchantButton);
        else
            ActiveButton.Instance.ResetButton();
    }
}
