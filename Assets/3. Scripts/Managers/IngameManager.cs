using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    private StageInfo Info;

    private void Awake()
    {
        Info = DataTableManager.Instance.GetStageInfo(GameManager.MyInstance.CurrentStageID);
        SetIngame(Info);
    }

    private void SetIngame(StageInfo info)
    {
        Instantiate(Resources.Load("Prefabs/Maps/" + info.MapPrefabs + "_" + Random.Range(1, 6)));
        GetComponent<EnemySpawn>().SetEnemySpawn(info.EnemyMaxNum, info.EnemyMinNum, info.ElitePercent, info.GuvPercent);
        EnemyPool.Instance.SetEnemyPool(DataTableManager.Instance.GetEnemyPrefabs(info.ID));
        InvadeGage.Instance.On(info.InvadeGage);
    }
}
