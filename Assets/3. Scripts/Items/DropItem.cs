using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    ItemBase item;
    [SerializeField]
    Text DI_Text;
    [SerializeField]
    SpriteRenderer sprite;

    private float speed;

    private Vector2 startPos;
    private Transform playerTransform;
    private float upTime;

    [HideInInspector]
    public bool L_Start;
    private bool up = false;
    DropItem (ItemBase item) //나중에 지울 가능성 큼
    {
        this.item = item;
        sprite.sprite = item.MyIcon;
    }
    public void SetDropItem(Item _item) // 몬스터에서 드랍할때 이걸로 추가할 예정
    {
        item = new ItemBase();
        item.itemInfo = _item;
        int a = Random.Range(0, 2);
        if (a == 0)
            item.MyQuality = Quality.Rare;
        else
            item.MyQuality = Quality.Epic;
        sprite.sprite = item.MyIcon;
        DI_Text.text = item.MyName;
    }
    private void Start()
    {
        speed = 0;
        playerTransform = GameObject.Find("Player").transform.GetChild(1).GetComponent<Transform>();
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            // 인벤토리에 아이템 추가
            InventoryScript.MyInstance.AddItem(item);

            // 아이템 획득 알림
            GameObject notice = Instantiate(Resources.Load("LootNotice") as GameObject, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
            notice.GetComponent<LootNotice>().SetDescript(item);
            notice.transform.SetParent(GameObject.Find("ItemLooting").transform);

            Destroy(gameObject);

        }
    }
    void Update()
    {
        if (L_Start)
        {
            if (!up)
            {
                Looting_Start();
            }
            else if(up )
            {
                Looting_ToPlayer();
            }
        }
    }

    public void Looting_Start()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(startPos.x, startPos.y + 0.3f),Time.deltaTime *3);
        upTime += Time.deltaTime;
        if(upTime >= 0.7f)
        {
            up = true;
        }
    }
    private void Looting_ToPlayer()
    {
        Vector2 dir = playerTransform.position - transform.position;
        speed += Time.deltaTime * 15;
        transform.Translate(dir.normalized * speed*Time.deltaTime);
    }
}
