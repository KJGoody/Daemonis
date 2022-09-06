using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    private void Awake()
    {
        InvadeGage.Instance.On();
    }
}
