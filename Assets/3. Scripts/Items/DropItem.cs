using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    ItemBase item;
    public ItemBase Item{ get { return item; } }
    [SerializeField]
    Text DI_Text;
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    Sprite goldImage;

    public enum IsKind
    {
        Gold, Item
    }
    public IsKind isKind;
    
    private int gold;
    private float speed;
    private Vector2 startPos;
    private Transform playerTransform;
    private float upTime;

    [HideInInspector]
    public bool L_Start;
    private bool up = false;

    public void SetDropItem(ItemInfo _item, Quality _quality) // 몬스터에서 드랍할때 이걸로 추가할 예정
    {
        isKind = IsKind.Item;
        item = new ItemBase();
        item.itemInfo = _item;
        item.quality = _quality;
        if(item.GetKind == Kinds.Equipment)
        {
            item.quality = (Quality)AddOptionManager.MyInstance.SetRandomEquipmentQuality();
            item.SetAddOption();
        }
        sprite.sprite = item.MyIcon;
        DI_Text.text = item.MyName;
    }

    public void SetGold(int _gold)
    {
        isKind = IsKind.Gold;
        gold = _gold;
        // DI_Text.color = new Color(171, 164, 36);
        DI_Text.text = _gold + " 골드";
        sprite.sprite = goldImage;
    }

    private void Start()
    {
        speed = 0;
        playerTransform = GameObject.Find("Player").transform.GetChild(1).GetComponent<Transform>();
        startPos = transform.position;
    }

    void Update()
    {
        if (L_Start)
        {
            if (!up)
                Looting_Start();
            else if (up)
                Looting_ToPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(isKind == IsKind.Gold || OptionPanel.MyInstance.lootingQuality[(int)item.quality]) // 골드 아니면 설정한 아이템 등급 아이템만 획득
            {
                GameObject notice = Instantiate(Resources.Load("LootNotice") as GameObject, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                notice.transform.SetParent(GameObject.Find("ItemLooting").transform);
                switch (isKind)
                {
                    case IsKind.Gold:
                        GameManager.MyInstance.DATA.Gold += gold;
                        notice.GetComponent<LootNotice>().SetGoldInfo(gold, goldImage);
                        break;
                    case IsKind.Item:
                        // 인벤토리에 아이템 추가
                        InventoryScript.MyInstance.AddItem(item);
                        // 아이템 획득 알림
                        notice.GetComponent<LootNotice>().SetDescript(item);
                        break;
                }
                Destroy(gameObject);
            }

        }
    }

    public void Looting_Start() // 루팅 시작
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(startPos.x, startPos.y + 0.3f), Time.deltaTime * 3);
        upTime += Time.deltaTime;
        if (upTime >= 0.7f)
        {
            up = true;
        }
    }
    private void Looting_ToPlayer() // 아이템 플레이어쪽으로
    {
        Vector2 dir = playerTransform.position - transform.position;
        speed += Time.deltaTime * 15;
        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }
}
