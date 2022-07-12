using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot_Base : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    public Image MyIcon
    {
        get { return icon; }
        set { icon = value; }
    }

    [SerializeField]
    private TextMeshProUGUI stackSize;
    public TextMeshProUGUI MyStackText { get { return stackSize; } }

    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }

}
