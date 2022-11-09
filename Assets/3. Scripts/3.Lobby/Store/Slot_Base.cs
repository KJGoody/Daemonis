using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot_Base : MonoBehaviour, IPointerClickHandler, IClickable
{
    [SerializeField] protected Image icon;
    [SerializeField] protected Image QualityImage;

    public Image MyIcon
    {
        get { return icon; }
        set { icon = value; }
    }

    public Image GetQualityImage
    {
        get { return QualityImage; }
    }

    public virtual void OnPointerClick(PointerEventData eventData) { }
}