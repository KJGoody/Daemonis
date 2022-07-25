using UnityEngine;
using UnityEngine.EventSystems;

public class CastingButton : ActionButton
{
    public IUseable Spell;

    private CastingButton AlreadySetButton;

    protected override void OnClick()
    {
        if(HandScript.MyInstance.MyMoveable == null)
        {
            // 액션퀵슬롯에 등록된 것이 사용할 수 있는거라면
            if (Spell != null)
            {
                // 현재 쿨타임이 0일 경우에만 사용할 수 있다. && 플레이어의 현재 마나가 스킬 마나보다 많아야 한다. && 공격중이 아니여야 한다.
                if (CurrentCollTime == 0 && Player.MyInstance.MyStat.CurrentMana - (Spell as Spell).ManaCost >= 0 && !Player.MyInstance.IsAttacking)
                {
                    CoolTime = (Spell as Spell).CoolTime;
                    StartCoroutine(StartCoolDown());

                    Player.MyInstance.MyStat.CurrentMana -= (Spell as Spell).ManaCost;

                    Spell.Use();
                }
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // 클릭이 발생했을 경우
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 손에 무언가 들려있을 경우
            if (HandScript.MyInstance.MyMoveable != null)
            {
                // IUseable 로 변환할 수 있는지 확인.
                if (HandScript.MyInstance.MyMoveable is IUseable && CurrentCollTime == 0)
                {
                    // 이미 ActionButton에 설정이 되어있는지 확인한다.
                    if (IsSetIUseable(HandScript.MyInstance.MyMoveable as IUseable))
                    {
                        if (AlreadySetButton.CurrentCollTime == 0)
                        {
                            // 이미 설정되어 있는 버튼을 초기화
                            AlreadySetButton.Spell = null;
                            AlreadySetButton.MyIcon.sprite = null;
                            AlreadySetButton.MyIcon.color = new Color(0, 0, 0, 0);
                            AlreadySetButton = null;

                            // 새로운 버튼에 할당
                            SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                            HandScript.MyInstance.SkillBlindControll();
                        }
                    }
                    // 설정되어 있지 않다면 정상적으로 등록한다.
                    else
                    {
                        SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                        if (HandScript.MyInstance.MyMoveable is Item_Base)
                            HandScript.MyInstance.ResetEquipPotion();
                        else
                            HandScript.MyInstance.SkillBlindControll();
                    }
                }
            }
        }
    }

    public override void SetUseable(IUseable useable)
    {
        Spell = useable;
        base.SetUseable(useable);
    }

    protected override bool IsSetIUseable(IUseable useable)
    {
        foreach (CastingButton castingButton in GameManager.MyInstance.CastingButtons)
        {
            if (castingButton.Spell != null)
                if (castingButton.Spell.GetName().Equals(useable.GetName()))
                {
                    AlreadySetButton = castingButton;
                    return true;
                }
        }
        return false;
    }
}
