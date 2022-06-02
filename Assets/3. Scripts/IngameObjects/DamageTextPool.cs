using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageTextPoolQueue
{
    public Queue<DamageText> damageTexts = new Queue<DamageText>();
}

public class DamageTextPool : MonoBehaviour
{
    private static DamageTextPool instance;
    public static DamageTextPool Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DamageTextPool>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject[] DamageTextPrefabs;
    [SerializeField]
    private DamageTextPoolQueue[] DamageTextPoolQueues;

    public enum DamageTextPrefabsName
    {
        Player = 0,
        Critical = 1,
        Enemy = 2
    }

    void Start()
    {
        Initialize(10, DamageTextPrefabsName.Player);
        Initialize(10, DamageTextPrefabsName.Critical);
        Initialize(10, DamageTextPrefabsName.Enemy);
    }

    private void Initialize(int initCount, DamageTextPrefabsName index)
    {
        for (int i = 0; i < initCount; i++)
            DamageTextPoolQueues[(int)index].damageTexts.Enqueue(CreateNewObject(index));
    }

    private DamageText CreateNewObject(DamageTextPrefabsName index)
    {
        DamageText newObj = Instantiate(DamageTextPrefabs[(int)index]).GetComponent<DamageText>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public DamageText GetObject(DamageTextPrefabsName index)
    {
        if (DamageTextPoolQueues[(int)index].damageTexts.Count > 0)
        {
            DamageText obj = DamageTextPoolQueues[(int)index].damageTexts.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            DamageText newObj = CreateNewObject(index);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(DamageText obj, DamageTextPrefabsName index)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        DamageTextPoolQueues[(int)index].damageTexts.Enqueue(obj);
    }
}
