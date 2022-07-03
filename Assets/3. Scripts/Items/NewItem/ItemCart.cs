using UnityEngine;
using UnityEngine.UI;

public class ItemCart : MonoBehaviour
{
    [SerializeField]
    private Item_Base item;
    public Item_Base Item { get { return item; } }
    [SerializeField]
    private Text ItemName;
    [SerializeField]
    private SpriteRenderer ItemSprite;
    [SerializeField]
    private Sprite GoldImage;

    public enum IsKind { Gold, Item }
    public IsKind isKind;

    private int GoldValue;
    private float Speed;
    private Vector2 StartPos;
    private Transform PlayerTransform;

    [HideInInspector]
    public bool IsLooting = false;
    private bool IsUp = false;
    private float UpTime = 0;

    private readonly float[] EquipmentQualityProb = new float[] { 5f, 4f, 3f, 2f, 1f, 0.5f };

    public void SetItem(ItemInfo_Base _item, Item_Base.Quality _quality) // 몬스터에서 드랍할때 이걸로 추가할 예정
    {
        isKind = IsKind.Item;
        item = new Item_Base();
        item.itemInfo = _item;
        item.quality = _quality;

        if (item.GetKind == ItemInfo_Base.Kinds.Equipment)
        {
            item.quality = (Item_Base.Quality)SetRandomEquipmentQuality();
            (item as Item_Equipment).SetAddOption();
        }

        ItemName.text = item.MyName;
        ItemSprite.sprite = item.MyIcon;
    }

    public int SetRandomEquipmentQuality() // 장비 퀄리티 랜덤
    {
        return (int)ChanceMaker.Choose(EquipmentQualityProb);
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
                        InventoryScript.MyInstance.AddItem(item);
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
