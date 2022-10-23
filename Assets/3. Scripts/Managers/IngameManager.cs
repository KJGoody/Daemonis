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
        // �������� �������� �ҷ���
        Instantiate(Resources.Load("Prefabs/Maps/" + Info.MapPrefabs + "_" + Random.Range(1, 6)));
        // ���� ��ȯ �⺻ ������ ������
        GetComponent<EnemySpawn>().SetEnemySpawn(Info.EnemyMaxNum, Info.EnemyMinNum, Info.ElitePercent, Info.GuvPercent);
        // ���� Ǯ�� �ش� �������� ���͸� �Է��Ѵ�.
        EnemyPool.Instance.SetEnemyPool(DataTableManager.Instance.GetEnemyPrefabs(Info.ID));
        // �κ��̵� �������� Ȱ��ȭ�Ѵ�.
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
