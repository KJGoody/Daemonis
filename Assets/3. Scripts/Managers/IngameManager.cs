using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    private StageInfo Info;
    private bool BossOnce;

    [HideInInspector] public static float StatPercent;

    private void Awake()
    {
        Info = DataTableManager.Instance.GetStageInfo(GameManager.MyInstance.CurrentStageID);
        // 스테이지 프리펩을 불러옴
        Instantiate(Resources.Load("Prefabs/Maps/" + Info.MapPrefabs + "_" + Random.Range(1, 6)));
        // 몬스터 소환 기본 설정을 설정함
        GetComponent<EnemySpawn>().SetEnemySpawn(Info.EnemyMaxNum, Info.EnemyMinNum, Info.ElitePercent, Info.GuvPercent);
        // 몬스터 풀에 해당 스테이지 몬스터를 입력한다.
        EnemyPool.Instance.SetEnemyPool(DataTableManager.Instance.GetEnemyPrefabs(Info.ID));
        // 인베이드 게이지를 활성화한다.
        InvadeGage.Instance.On(Info.InvadeGage);
        StatPercent = Info.EnemyStatPercent;
    }

    private void Update()
    {
        if (InvadeGage.Instance.IsBossTime && BossOnce == false)
        {
            BossOnce = true;
            EnemyBoss Boss = Instantiate(Info.BossInfo.Prefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<EnemyBoss>();
            Boss.enemytype.SetInfo = Info.BossInfo;
            Boss.DropTime = Info.DropTime;
        }
    }
}
