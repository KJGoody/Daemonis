using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GNode
{
    public enum LayerTypes { None, Floor, Water, Wall };
    public LayerTypes LayerType;
    public Vector3 WorldPos;
    public int Gridx;
    public int Gridy;

    public GNode(LayerTypes layerType, Vector3 worldPos, int gridx, int gridy)
    {
        LayerType = layerType;
        WorldPos = worldPos;
        Gridx = gridx;
        Gridy = gridy;
    }
}

public class EnemySpawn : MonoBehaviour
{
    private int MaxEnemyNum;
    private int LimitCurrentEnemyNum;
    [HideInInspector] public int CurrentEnemyNum;

    [HideInInspector] public int TotalEliteNum;
    [HideInInspector] public int CurrnentEliteNum;

    [HideInInspector] public int TotalGuvNum = 0;
    [HideInInspector] public int CurrnetGuvNum;

    private bool EndSpawn = false;

    private GNode[,] Grid;
    private Vector3 GridCenter;
    [SerializeField] private Vector2 GridSize;    // �׸��� ũ��
    private int GridSizeX;      // �׸��� x ũ��
    private int GridSizeY;      // �׸��� y ũ��
    private readonly float Radius = 0.5f;

    private void CreateGrid()
    {
        GridCenter = transform.position;
        GridSizeX = Mathf.RoundToInt(GridSize.x);   // �׸����� ���� ũ��
        GridSizeY = Mathf.RoundToInt(GridSize.y);   // �׸����� ���� ũ��

        Grid = new GNode[GridSizeX, GridSizeY];
        Vector3 worldBottomLeft = GridCenter - Vector3.right * GridSize.x / 2 - Vector3.up * GridSizeY / 2;    // ���� ��ġ���� ���ʾƷ� ��ǥ�� ����
        Vector3 worldPosition;
        for (int x = 0; x < GridSizeX; x++)
            for (int y = 0; y < GridSizeY; y++)
            {
                worldPosition = worldBottomLeft + Vector3.right * (x + Radius) + Vector3.up * (y + Radius);
                GNode.LayerTypes layerType;
                if (Physics2D.OverlapCircle(worldPosition, Radius - 0.1f, LayerMask.GetMask("Floor")))    // �ش� ����� ���̾� Ȯ��
                    layerType = GNode.LayerTypes.Floor;
                else if (Physics2D.OverlapCircle(worldPosition, Radius - 0.1f, LayerMask.GetMask("Wall")))    // �ش� ����� ���̾� Ȯ��
                    layerType = GNode.LayerTypes.Wall;
                else if (Physics2D.OverlapCircle(worldPosition, Radius - 0.1f, LayerMask.GetMask("Water")))    // �ش� ����� ���̾� Ȯ��
                    layerType = GNode.LayerTypes.Water;
                else
                    layerType = GNode.LayerTypes.None;
                Grid[x, y] = new GNode(layerType, worldPosition, x, y);
            }
    }

    private void Start()
    {
        MaxEnemyNum = 80;
        LimitCurrentEnemyNum = 20;
        TotalEliteNum = Random.Range(0, 3 + 1);
        if (ChanceMaker.GetThisChanceResult_Percentage(25))
            TotalGuvNum = 1;
        CreateGrid();
        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
        if (EndSpawn)
            if (LimitCurrentEnemyNum > CurrentEnemyNum)
                StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (CurrentEnemyNum < MaxEnemyNum)
        {
            GNode newStartPosition;
            do
            {
                newStartPosition = Grid[Random.Range(0, GridSizeX), Random.Range(0, GridSizeY)];
            } while (newStartPosition.LayerType != GNode.LayerTypes.Floor);

            PositioningEnemy(newStartPosition.WorldPos);

            CurrentEnemyNum++;
            yield return new WaitForSeconds(0.01f);
        }
        EndSpawn = true;
    }

    private void PositioningEnemy(Vector3 newworldposition)
    {
        if (TotalGuvNum > CurrnetGuvNum)
        {
            if (ChanceMaker.GetThisChanceResult_Percentage(50))
                EnemyPool.Instance.GetObject(EnemyPool.MonsterPrefabName.Kobold_Melee_Guv).PositioningEnemyBase(this, newworldposition);
            else
                EnemyPool.Instance.GetObject(EnemyPool.MonsterPrefabName.Kobold_Ranged_Guv).PositioningEnemyBase(this, newworldposition);

            CurrnetGuvNum += 1;
            return;
        }

        if (TotalEliteNum > CurrnentEliteNum)
        {
            if (ChanceMaker.GetThisChanceResult_Percentage(50))
                EnemyPool.Instance.GetObject(EnemyPool.MonsterPrefabName.Kobold_Melee_Elite).PositioningEnemyBase(this, newworldposition);
            else
                EnemyPool.Instance.GetObject(EnemyPool.MonsterPrefabName.Kobold_Ranged_Elite).PositioningEnemyBase(this, newworldposition);

            CurrnentEliteNum += 1;
            return;
        }

        if (ChanceMaker.GetThisChanceResult_Percentage(50))
            EnemyPool.Instance.GetObject(EnemyPool.MonsterPrefabName.Kobold_Melee).PositioningEnemyBase(this, newworldposition);
        else
            EnemyPool.Instance.GetObject(EnemyPool.MonsterPrefabName.Kobold_Ranged).PositioningEnemyBase(this, newworldposition);
    }
}
