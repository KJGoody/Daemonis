using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewTextPoolQueue
{
    public Queue<NewText> damageTexts = new Queue<NewText>();
}

public class NewTextPool : MonoBehaviour
{
    private static NewTextPool instance;
    public static NewTextPool Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<NewTextPool>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject[] DamageTextPrefabs;
    [SerializeField]
    private NewTextPoolQueue[] DamageTextPoolQueues;

    public enum NewTextPrefabsName
    {
        Player = 0,
        Critical = 1,
        Enemy = 2,
        Heal = 3
    }

    void Start()
    {
        Initialize(10, NewTextPrefabsName.Player);
        Initialize(10, NewTextPrefabsName.Critical);
        Initialize(10, NewTextPrefabsName.Enemy);
        Initialize(5, NewTextPrefabsName.Heal);
    }

    private void Initialize(int initCount, NewTextPrefabsName index)
    {
        for (int i = 0; i < initCount; i++)
            DamageTextPoolQueues[(int)index].damageTexts.Enqueue(CreateNewObject(index));
    }

    private NewText CreateNewObject(NewTextPrefabsName index)
    {
        NewText newObj = Instantiate(DamageTextPrefabs[(int)index]).GetComponent<NewText>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public NewText GetObject(NewTextPrefabsName index)
    {
        if (DamageTextPoolQueues[(int)index].damageTexts.Count > 0)
        {
            NewText obj = DamageTextPoolQueues[(int)index].damageTexts.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            NewText newObj = CreateNewObject(index);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(NewText obj, NewTextPrefabsName index)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        DamageTextPoolQueues[(int)index].damageTexts.Enqueue(obj);
    }
}
