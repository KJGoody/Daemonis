using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> BuffList = new List<GameObject>();
    [SerializeField] private Buff[] buffs;

    public void AddBuffImage(string BuffName, Character target)
    {
        Buff buff = Array.Find(buffs, source => source.BuffName == BuffName);
        GameObject Buff = Instantiate(buff.gameObject, transform);
        BuffList.Add(Buff);
        Buff.GetComponent<Buff>().ExecuteBuff(this, target);
    }
}
