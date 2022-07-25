using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootNotice : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;

    private Coroutine CurrentCoroutine;

    private void Awake()
    {
        LootNoticeManager.MyInstance.AddLootNotice(gameObject);
    }

    public void SetDescript(Item_Base _Item) // ���� ����
    {
        image.sprite = _Item.Icon;
        text.text = _Item.MyName;
    }

    public void SetGoldInfo(int _gold, Sprite _sprite) // ��� ���� ����
    {
        image.sprite = _sprite;
        text.text = _gold + " ���";
    }

    void Start()
    {
        CurrentCoroutine = StartCoroutine(NoticeOff(3));
    }

    public IEnumerator NoticeOff(float Seconds = 0)
    {
        if (Seconds == 0)
            StopCoroutine(CurrentCoroutine);

        yield return new WaitForSeconds(Seconds);

        anim.SetTrigger("Off");
        LootNoticeManager.MyInstance.noticeList.Remove(gameObject);
    }
}
