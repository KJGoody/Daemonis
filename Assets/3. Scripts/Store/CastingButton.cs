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
            // �׼������Կ� ��ϵ� ���� ����� �� �ִ°Ŷ��
            if (Spell != null)
            {
                // ���� ��Ÿ���� 0�� ��쿡�� ����� �� �ִ�. && �÷��̾��� ���� ������ ��ų �������� ���ƾ� �Ѵ�. && �������� �ƴϿ��� �Ѵ�.
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
        // Ŭ���� �߻����� ���
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // �տ� ���� ������� ���
            if (HandScript.MyInstance.MyMoveable != null)
            {
                // IUseable �� ��ȯ�� �� �ִ��� Ȯ��.
                if (HandScript.MyInstance.MyMoveable is IUseable && CurrentCollTime == 0)
                {
                    // �̹� ActionButton�� ������ �Ǿ��ִ��� Ȯ���Ѵ�.
                    if (IsSetIUseable(HandScript.MyInstance.MyMoveable as IUseable))
                    {
                        if (AlreadySetButton.CurrentCollTime == 0)
                        {
                            // �̹� �����Ǿ� �ִ� ��ư�� �ʱ�ȭ
                            AlreadySetButton.Spell = null;
                            AlreadySetButton.MyIcon.sprite = null;
                            AlreadySetButton.MyIcon.color = new Color(0, 0, 0, 0);
                            AlreadySetButton = null;

                            // ���ο� ��ư�� �Ҵ�
                            SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                            HandScript.MyInstance.SkillBlindControll();
                        }
                    }
                    // �����Ǿ� ���� �ʴٸ� ���������� ����Ѵ�.
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
