using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private static BuffManager instance;
    public static BuffManager myInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BuffManager>();

            return instance;
        }
    }

    public List<GameObject> BuffList = new List<GameObject>();

    public void AddBuffImage(Character target)
    {
        GameObject buff = Instantiate(Resources.Load("Buff/BaseBuff") as GameObject, transform);
        BuffList.Add(buff);
        buff.GetComponent<Buff>().ExecuteBuff(target);
    }

}
