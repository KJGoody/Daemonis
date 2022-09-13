using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPoolQueue
{
    public Queue<EnemyBase> EnemyScripts;

    public EnemyPoolQueue()
    {
        EnemyScripts = new Queue<EnemyBase>();
    }
}

public class EnemyPool : MonoBehaviour
{
    private static EnemyPool instance;
    public static EnemyPool Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<EnemyPool>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject[] MonsterPrefab;
    [SerializeField]
    private EnemyPoolQueue[] EnemyPoolQueues;

    public enum MonsterPrefabName
    {
        Kobold_Melee = 0,
        Kobold_Melee_Elite = 1,
        Kobold_Melee_Guv = 2,
        Kobold_Ranged = 3,
        Kobold_Ranged_Elite = 4,
        Kobold_Ranged_Guv = 5
    }

    private void Awake()
    {
        EnemyPoolQueues = new EnemyPoolQueue[MonsterPrefab.Length];
        for (int i = 0; i < EnemyPoolQueues.Length; i++)
            EnemyPoolQueues[i] = new EnemyPoolQueue();

        Initialize(20, MonsterPrefabName.Kobold_Melee);
        Initialize(10, MonsterPrefabName.Kobold_Melee_Elite);
        Initialize(5, MonsterPrefabName.Kobold_Melee_Guv);
        Initialize(20, MonsterPrefabName.Kobold_Ranged);
        Initialize(10, MonsterPrefabName.Kobold_Ranged_Elite);
        Initialize(5, MonsterPrefabName.Kobold_Ranged_Guv);
    }

    private void Initialize(int initCount, MonsterPrefabName index)
    {
        for (int i = 0; i < initCount; i++)
            EnemyPoolQueues[(int)index].EnemyScripts.Enqueue(CreateNewObject(index));
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
        if (EnemyPoolQueues[(int)index].EnemyScripts.Count > 0)
        {
            var obj = EnemyPoolQueues[(int)index].EnemyScripts.Dequeue();
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
        EnemyPoolQueues[(int)index].EnemyScripts.Enqueue(obj);
    }
}
