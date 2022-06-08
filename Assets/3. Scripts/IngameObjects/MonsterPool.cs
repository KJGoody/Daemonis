using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterPoolQueue
{
    public Queue<EnemyBase> EnemyScripts;

    public MonsterPoolQueue()
    {
        EnemyScripts = new Queue<EnemyBase>();
    }
}

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
    private GameObject[] MonsterPrefab;
    private MonsterPoolQueue[] MonsterPoolQueues;

    public enum MonsterPrefabName
    {
        Kobold_Melee,
        Kobold_Ranged
    }

    private void Awake()
    {
        MonsterPoolQueues = new MonsterPoolQueue[MonsterPrefab.Length];
        for (int i = 0; i < MonsterPoolQueues.Length; i++)
            MonsterPoolQueues[i] = new MonsterPoolQueue();
        Initialize(20, MonsterPrefabName.Kobold_Melee);
        Initialize(20, MonsterPrefabName.Kobold_Ranged);
    }

    private void Initialize(int initCount, MonsterPrefabName index)
    {
        for (int i = 0; i < initCount; i++)
            MonsterPoolQueues[(int)index].EnemyScripts.Enqueue(CreateNewObject(index));
    }

    private EnemyBase CreateNewObject(MonsterPrefabName index)
    {
        var newObj = Instantiate(MonsterPrefab[(int)index]).GetComponent<EnemyBase>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public EnemyBase GetObject(MonsterPrefabName index)
    {
        if (MonsterPoolQueues[(int)index].EnemyScripts.Count > 0)
        {
            var obj = MonsterPoolQueues[(int)index].EnemyScripts.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject(index);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(EnemyBase obj, MonsterPrefabName index)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        MonsterPoolQueues[(int)index].EnemyScripts.Enqueue(obj);
    }
}
