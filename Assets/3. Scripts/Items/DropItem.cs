using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    Item item;
    [SerializeField]
    Text DI_Text;
    SpriteRenderer sprite;

    private float speed;

    private Vector2 startPos;
    private Transform playerTransform;
    private float upTime;

    [HideInInspector]
    public bool L_Start;
    private bool up = false;
    DropItem (Item item)
    {
        this.item = item;
        sprite.sprite = item.MyIcon;
    }
    private void Start()
    {
        DI_Text.text = item.MyName;
        speed = 0;
        playerTransform = GameObject.Find("Player").transform.GetChild(1).GetComponent<Transform>();
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            Debug.Log(item.MyName + "À» È¹µæÇÏ¿´½À´Ï´Ù");
            InventoryScript.MyInstance.AddItem(item);
            Destroy(gameObject);
        }
    }
    void Update()
    {
        Debug.Log(playerTransform);
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
