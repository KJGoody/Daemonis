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

    [SerializeField] private List<GameObject> EnemyPrefabsArray;
    [HideInInspector] public int BaseNum;
    private EnemyPoolQueue[] EnemyPoolQueues;

    public void SetEnemyPool(List<GameObject> array)
    {
        EnemyPrefabsArray = array;
        EnemyPoolQueues = new EnemyPoolQueue[EnemyPrefabsArray.Count];
        for (int i = 0; i < EnemyPoolQueues.Length; i++)
        {
            EnemyPoolQueues[i] = new EnemyPoolQueue();
            Initialize(5, i);
            string[] NameSplite = array[i].name.Split('_');
            if (NameSplite[NameSplite.Length - 1] == "Normal")
                BaseNum++;
        }

        StartCoroutine(GetComponent<EnemySpawn>().SpawnEnemy());
    }

    public int GetIndex(GameObject prefab)
    {
        for (int i = 0; i < EnemyPrefabsArray.Count; i++)
            if (EnemyPrefabsArray[i] == prefab)
                return i;

        return 0;
    }

    private void Initialize(int initCount, int index)
    {
        for (int i = 0; i < initCount; i++)
            EnemyPoolQueues[index].EnemyScripts.Enqueue(CreateNewObject(index));
    }

    private EnemyBase CreateNewObject(int index)
    {
        var newObj = Instantiate(EnemyPrefabsArray[index]).GetComponent<EnemyBase>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public EnemyBase GetObject(int index)
    {
        if (EnemyPoolQueues[index].EnemyScripts.Count > 0)
        {
            var obj = EnemyPoolQueues[index].EnemyScripts.Dequeue();
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

    public void ReturnObject(EnemyBase obj, int index)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        EnemyPoolQueues[index].EnemyScripts.Enqueue(obj);
    }
}
