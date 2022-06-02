using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    private static MonsterPool instance;
    public static MonsterPool Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<MonsterPool>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject MonsterPrefab;

    Queue<EnemyBase> poolingObjectQueue = new Queue<EnemyBase>();

    private void Awake()
    {
        Initialize(20);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
            poolingObjectQueue.Enqueue(CreateNewObject());
    }

    private EnemyBase CreateNewObject()
    {
        var newObj = Instantiate(MonsterPrefab).GetComponent<EnemyBase>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public EnemyBase GetObject()
    {
        if (poolingObjectQueue.Count > 0)
        {
            var obj = poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(EnemyBase obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}
