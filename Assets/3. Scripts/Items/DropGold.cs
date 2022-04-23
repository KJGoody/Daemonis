using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DropGold : MonoBehaviour
{
    [SerializeField]
    Text DG_Text;
    private int gold;
    [SerializeField]
    Sprite sprite;

    private float speed;

    private Vector2 startPos;
    private Transform playerTransform;
    private float upTime;

    [HideInInspector]
    public bool L_Start;
    private bool up = false;
    public void SetGold(int _gold)
    {
        gold = _gold;
        DG_Text.text = _gold + " °ñµå";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            GameManager.MyInstance.MyGold += gold;
            GameObject notice = Instantiate(Resources.Load("LootNotice") as GameObject, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
            notice.GetComponent<LootNotice>().SetGoldInfo(gold,sprite);
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
            else if (up)
            {
                Looting_ToPlayer();
            }
        }
    }

    public void Looting_Start()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(startPos.x, startPos.y + 0.3f), Time.deltaTime * 3);
        upTime += Time.deltaTime;
        if (upTime >= 0.7f)
        {
            up = true;
        }
    }
    private void Looting_ToPlayer()
    {
        Vector2 dir = playerTransform.position - transform.position;
        speed += Time.deltaTime * 15;
        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }
}
