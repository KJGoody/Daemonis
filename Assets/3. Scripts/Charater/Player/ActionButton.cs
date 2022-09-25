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
        // 클릭 이벤트를 MyButton 에 등록한다.
        MyButton.onClick.AddListener(OnClick);
    }

    // 클릭 발생하면 실행
    protected abstract void OnClick();

    // 클릭이 발생했는지 감지. 
    // IPointerClickHandler 에 명시된 함수이다.
    public virtual void SetUseable(IUseable useable) { UpdateVisual(useable); }

    protected virtual void UpdateVisual(IUseable useable)
    {
        // ActionButton의 이미지를 변경한다.
        MyIcon.sprite = (useable as IMoveable).Icon;
        MyIcon.color = Color.white;
    }

    protected IEnumerator StartCoolDown()
    {   // 재사용 대기시간 이미지를 보이도록 하는 코루틴
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

    // IUseable이 이미 설정이 되어있는지 확인하는 함수
    protected abstract bool IsSetIUseable(IUseable useable);
}