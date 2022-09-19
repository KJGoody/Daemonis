using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuffPoolQueue
{
    public Queue<Puff> puffScripts;

    public PuffPoolQueue()
    {
        puffScripts = new Queue<Puff>();
    }
}

public class PuffPool : MonoBehaviour
{
    private static PuffPool instance;
    public static PuffPool Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PuffPool>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject[] PuffPrefabs;
    private PuffPoolQueue[] puffPoolQueues;

    public enum PuffPrefabsName
    {
        Hit_01 = 0,
        Fire = 1
    }
    
    void Start()
    {
        puffPoolQueues = new PuffPoolQueue[PuffPrefabs.Length];
        for (int i = 0; i < puffPoolQueues.Length; i++)
            puffPoolQueues[i] = new PuffPoolQueue();

        Initialize(10, PuffPrefabsName.Hit_01);
        Initialize(10, PuffPrefabsName.Fire);
    }
    
    private void Initialize(int initCount, PuffPrefabsName index)
    {
        for (int i = 0; i < initCount; i++)
            puffPoolQueues[(int)index].puffScripts.Enqueue(CreateNewObject(index));
    }

    private Puff CreateNewObject(PuffPrefabsName index)
    {
        Puff newObj = Instantiate(PuffPrefabs[(int)index]).GetComponent<Puff>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public Puff GetObject(PuffPrefabsName index)
    {
        if (puffPoolQueues[(int)index].puffScripts.Count > 0)
        {
            Puff obj = puffPoolQueues[(int)index].puffScripts.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            Puff newObj = CreateNewObject(index);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(Puff obj, PuffPrefabsName index)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        puffPoolQueues[(int)index].puffScripts.Enqueue(obj);
    }
}
