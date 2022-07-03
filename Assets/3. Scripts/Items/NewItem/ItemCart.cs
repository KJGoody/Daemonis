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

    public void SetItem(ItemInfo_Base _item, Item_Base.Quality _quality) // ���Ϳ��� ����Ҷ� �̰ɷ� �߰��� ����
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

    public int SetRandomEquipmentQuality() // ��� ����Ƽ ����
    {
        return (int)ChanceMaker.Choose(EquipmentQualityProb);
    }

    public void SetGold(int _gold)
    {
        isKind = IsKind.Gold;
        GoldValue = _gold;
        ItemName.text = _gold + " ���";
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
            // ��� Or ������ ������ ��� �����۸� ȹ��
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
                        // �κ��丮�� ������ �߰�
                        InventoryScript.MyInstance.AddItem(item);
                        // ������ ȹ�� �˸�
                        notice.GetComponent<LootNotice>().SetDescript(item);
                        break;
                }
                Destroy(gameObject);
            }

        }
    }

    public void ItemUp() // ���� ����
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(StartPos.x, StartPos.y + 0.3f), Time.deltaTime * 3);

        UpTime += Time.deltaTime;
        if (UpTime >= 0.7f)
            IsUp = true;
    }
    private void LootingToPlayer() // ������ �÷��̾�������
    {
        Speed += Time.deltaTime * 15;
        Vector2 dir = PlayerTransform.position - transform.position;
        transform.Translate(dir.normalized * Speed * Time.deltaTime);
    }
}
