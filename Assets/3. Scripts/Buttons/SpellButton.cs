using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string spellName;

    // ��ư�� Ŭ���ϸ� OnPointerClick �� ȣ��ȴ�.
    public void OnPointerClick(PointerEventData eventData)
    {
        // ���� ���콺�� �����ٸ�
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //HandScript.MyInstance.TakeMoveable(SpellBook.MyInstance.GetSpell(spellName));
        }
    }
    
}