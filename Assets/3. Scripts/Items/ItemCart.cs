using UnityEngine;
using UnityEngine.UI;

public class ItemCart : MonoBehaviour
{
    private Item_Base item;
    public Item_Base Item { get { return item; } }

    [SerializeField] private SpriteRenderer S_Item;
    [SerializeField] private SpriteRenderer S_Quality;

    public enum Kinds { Gold, Item }
    [HideInInspector] public Kinds Kind;

    private int GoldValue;
    private float Speed;
    private Vector2 StartPos;
    private Transform PlayerTransform;

    [HideInInspector] public bool IsLooting = false;
    private bool IsUp = false;
    private float UpTime = 0;

    public void SetItem_Consumable(ItemInfo_Consumable ItemInfo, Item_Base.Qualitys quality)
    {
        Kind = Kinds.Item;

        string[] kind = ItemInfo.ID.Split('_');
        switch (kind[1])
        {
            case "Potion":
                item = new Item_Potion();
                (item as Item_Potion).SetInfo(ItemInfo as ItemInfo_Potion);
                item.Quality = quality;
                break;
        }

        S_Item.sprite = item.Icon;
        S_Quality.color = item.GetQualityColor;
    }

    public void SetItem_Equipment(ItemInfo_Equipment ItemInfo, Item_Base.Qualitys quality)
    {
        Kind = Kinds.Item;

        item = new Item_Equipment();
        (item as Item_Equipment).SetInfo(ItemInfo);
        item.Quality = quality;
        (item as Item_Equipment).SetAddOption();

        S_Item.sprite = item.Icon;
        S_Quality.color = item.GetQualityColor;
    }

    public void SetGold(int Value)
    {
        Kind = Kinds.Gold;
        GoldValue = Value;

        S_Item.sprite = Resources.Load<Sprite>("Sprites/S_Gold");
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
        if (collision.CompareTag("Player") && IsLooting)
        {
            // 골드 Or 설정한 아이템 등급 아이템만 획득
            if (Kind == Kinds.Gold || OptionPanel.MyInstance.lootingQuality[(int)item.Quality])
            {
                GameObject notice = Instantiate(Resources.Load("LootNotice") as GameObject, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                notice.transform.SetParent(GameObject.Find("ItemLooting").transform);

                switch (Kind)
                {
                    case Kinds.Gold:
                        GameManager.MyInstance.DATA.Gold += GoldValue;
                        notice.GetComponent<LootNotice>().SetGoldInfo(GoldValue, Resources.Load<Sprite>("Sprites/S_Gold"));
                        break;

                    case Kinds.Item:
                        // 인벤토리에 아이템 추가
                        switch (item.Kind)
                        {
                            case ItemInfo_Base.Kinds.Equipment:
                                InventoryScript.MyInstance.AddItem(item);
                                Looting.Instance.LootingSlotNum--;
                                break;

                            case ItemInfo_Base.Kinds.Potion:
                                InventoryScript.MyInstance.AddItem(item, true);
                                Looting.Instance.LootingCansStackNum--;
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
