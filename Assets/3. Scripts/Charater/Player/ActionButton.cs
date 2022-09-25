using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ActionButton : Slot_Base
{
    public Button MyButton { get; private set; }

    [SerializeField]
    protected Image CoolTimeFillImage;
    protected float CoolTime;
    protected float CurrentCollTime = 0f;

    protected virtual void Start()
    {
        MyButton = GetComponent<Button>();
        // Ŭ�� �̺�Ʈ�� MyButton �� ����Ѵ�.
        MyButton.onClick.AddListener(OnClick);
    }

    // Ŭ�� �߻��ϸ� ����
    protected abstract void OnClick();

    // Ŭ���� �߻��ߴ��� ����. 
    // IPointerClickHandler �� ��õ� �Լ��̴�.
    public virtual void SetUseable(IUseable useable) { UpdateVisual(useable); }

    protected virtual void UpdateVisual(IUseable useable)
    {
        // ActionButton�� �̹����� �����Ѵ�.
        MyIcon.sprite = (useable as IMoveable).Icon;
        MyIcon.color = Color.white;
    }

    protected IEnumerator StartCoolDown()
    {   // ���� ���ð� �̹����� ���̵��� �ϴ� �ڷ�ƾ
        CoolTimeFillImage.gameObject.SetActive(true);

        CurrentCollTime = CoolTime;
        while (CurrentCollTime > 0)
        {
            CurrentCollTime -= 0.1f;
            CoolTimeFillImage.fillAmount = CurrentCollTime / CoolTime;
            yield return new WaitForSeconds(0.1f);
        }
        CoolTimeFillImage.fillAmount = 0;
        CurrentCollTime = 0;

        CoolTimeFillImage.gameObject.SetActive(false);
    }

    // IUseable�� �̹� ������ �Ǿ��ִ��� Ȯ���ϴ� �Լ�
    protected abstract bool IsSetIUseable(IUseable useable);
}