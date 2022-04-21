using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public DropItem dropItem;
    public ItemBase a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void spawn()
    {
        Instantiate(dropItem, gameObject.transform);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
