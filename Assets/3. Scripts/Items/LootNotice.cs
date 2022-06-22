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
    public void SetDescript(ItemBase _Item) // Á¤º¸ ¼³Á¤
    {
        image.sprite = _Item.MyIcon;
        text.text = _Item.MyName;
    }
    public void SetGoldInfo(int _gold, Sprite _sprite) // °ñµå Á¤º¸ ¼³Á¤
    {
        image.sprite = _sprite;
        text.text = _gold + " °ñµå";
    }
    void Start()
    {
        Invoke("Off", 3);
    }
    public void  Off() // »ç¶óÁü
    {
        anim.SetTrigger("Off");
        LootNoticeManager.MyInstance.noticeList.Remove(gameObject);

    }
}
