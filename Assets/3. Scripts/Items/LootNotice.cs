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

    private void Awake()
    {
        LootNoticeManager.MyInstance.AddLootNotice(gameObject);
    }

    public void SetDescript(Item_Base _Item) // ���� ����
    {
        image.sprite = _Item.MyIcon;
        text.text = _Item.MyName;
    }

    public void SetGoldInfo(int _gold, Sprite _sprite) // ��� ���� ����
    {
        image.sprite = _sprite;
        text.text = _gold + " ���";
    }

    void Start()
    {
        StartCoroutine(NoticeOff(3));
    }

    public IEnumerator NoticeOff(float Seconds = 0)
    {
        yield return new WaitForSeconds(Seconds);

        anim.SetTrigger("Off");
        LootNoticeManager.MyInstance.noticeList.Remove(gameObject);
    }
}
