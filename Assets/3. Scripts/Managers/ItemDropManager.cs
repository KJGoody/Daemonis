using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public DropItem dropItem;
    public Item aa;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawn",0,1);
    }

    void spawn()
    {
        DropItem a = Instantiate(dropItem, gameObject.transform.position, Quaternion.identity).GetComponent<DropItem>();
        a.SetDropItem(aa);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
