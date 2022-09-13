using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot_Base : MonoBehaviour, IPointerClickHandler, IClickable
{
    [SerializeField]
    protected Image icon;
    public Image MyIcon
    {
        get { return icon; }
        set { icon = value; }
    }

    public virtual void OnPointerClick(PointerEventData eventData) { }
}