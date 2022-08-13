using UnityEngine;
using UnityEngine.UI;

public class ItemCart : MonoBehaviour
{
    private Item_Base item;
    public Item_Base Item { get { return item; } }

    private Item_Equipment item_Equipment;
    private Item_Consumable item_Consumable;

    [SerializeField] private Text ItemName;
    [SerializeField] private SpriteRenderer ItemSprite;
    [SerializeField] private Sprite GoldImage;

    public enum IsKind { Gold, Item }
    public IsKind isKind;

    private int GoldValue;
    private float Speed;
    private Vector2 StartPos;
    private Transform PlayerTransform;

    [HideInInspector] public bool IsLooting = false;
    private bool IsUp = false;
    private float UpTime = 0;

    public void SetItem_Consumable(ItemInfo_Consumable ItemInfo, Item_Base.Quality quality)
    {
        isKind = IsKind.Item;

        string[] kind = ItemInfo.ID.Split('_');
        switch (kind[1])
        {
            case "Potion":
                item_Consumable = new Item_Potion();
                (item_Consumable as Item_Potion).itemInfo = ItemInfo as ItemInfo_Potion;
                item_Consumable.quality = quality;
                break;
        }

        item = item_Consumable;
        ItemName.text = item_Consumable.Name;
        ItemSprite.sprite = item_Consumable.Icon;
    }

    public void SetItem_Equipment(ItemInfo_Equipment ItemInfo, Item_Base.Quality quality)
    {
        isKind = IsKind.Item;
        item_Equipment = new Item_Equipment();
        item_Equipment.itemInfo = ItemInfo;
        item_Equipment.quality = quality;
        item_Equipment.SetAddOption();

        item = item_Equipment;
        ItemName.text = item_Equipment.Name;
        ItemSprite.sprite = item_Equipment.Icon;
    }

    public void SetGold(int _gold)
    {
        isKind = IsKind.Gold;
        GoldValue = _gold;
        ItemName.text = _gold + " 골드";
        ItemSprite.sprite = GoldImage;
    }

    private void Start()
    {
        Speed = 0;
        PlayerTransform = GameObject.Find("Player").transform.GetChild(1).GetComponent<Transform>();
        StartPos = transform.position;
    }

    private void Update()
    {
        if (IsLooting)
        {
            if (!IsUp)
                ItemUp();
            else
                LootingToPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 골드 Or 설정한 아이템 등급 아이템만 획득
            if (isKind == IsKind.Gold || OptionPanel.MyInstance.lootingQuality[(int)item.quality])
            {
                GameObject notice = Instantiate(Resources.Load("LootNotice") as GameObject, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                notice.transform.SetParent(GameObject.Find("ItemLooting").transform);

                switch (isKind)
                {
                    case IsKind.Gold:
                        GameManager.MyInstance.DATA.Gold += GoldValue;
                        notice.GetComponent<LootNotice>().SetGoldInfo(GoldValue, GoldImage);
                        break;

                    case IsKind.Item:
                        // 인벤토리에 아이템 추가
                        switch (item.Kind)
                        {
                            case ItemInfo_Base.Kinds.Potion:
                                InventoryScript.MyInstance.AddItem(item, true);
                                break;

                            default:
                                InventoryScript.MyInstance.AddItem(item);
                                break;
                        }
                        // 아이템 획득 알림
                        notice.GetComponent<LootNotice>().SetDescript(item);
                        break;
                }
                Destroy(gameObject);
            }

        }
    }

    public void ItemUp() // 루팅 시작
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(StartPos.x, StartPos.y + 0.3f), Time.deltaTime * 3);

        UpTime += Time.deltaTime;
        if (UpTime >= 0.7f)
            IsUp = true;
    }

    private void LootingToPlayer() // 아이템 플레이어쪽으로
    {
        Speed += Time.deltaTime * 15;
        Vector2 dir = PlayerTransform.position - transform.position;
        transform.Translate(dir.normalized * Speed * Time.deltaTime);
    }
}
