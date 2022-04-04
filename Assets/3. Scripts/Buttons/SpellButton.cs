using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string spellName;

    // 버튼을 클릭하면 OnPointerClick 이 호출된다.
    public void OnPointerClick(PointerEventData eventData)
    {
        // 왼쪽 마우스가 눌린다면
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //HandScript.MyInstance.TakeMoveable(SpellBook.MyInstance.GetSpell(spellName));
        }
    }
    
}