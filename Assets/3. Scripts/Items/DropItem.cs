using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    Item item;
    SpriteRenderer sprite;
    DropItem (Item item)
    {
        this.item = item;
        sprite.sprite = item.MyIcon;
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
        
    }
}
