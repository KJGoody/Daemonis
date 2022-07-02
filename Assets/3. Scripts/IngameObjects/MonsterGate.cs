using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GNode
{
    public bool IsWall;
    public Vector3 WorldPos;
    public int Gridx;
    public int Gridy;

    public GNode(bool isWall, Vector3 worldPos, int gridx, int gridy)
    {
        IsWall = isWall;
        WorldPos = worldPos;
        Gridx = gridx;
        Gridy = gridy;
    }
}

public class MonsterGate : MonoBehaviour
{
    private int TotalEnemyNum;
    [HideInInspector]
    public int CurrentEnemyNum;
    [HideInInspector]
    public int DeathEnemyNum = 0;

    private int TotalEliteNum;
    [HideInInspector]
    public int CurrnentEliteNum;
    [HideInInspector]
    public int DeathEliteNum = 0;

    private int TotalGuvNum = 0;
    [HideInInspector]
    public int CurrnetGuvNum;
    [HideInInspector]
    public int DeathGuvNum = 0;

    private GNode[,] Grid;
    private Vector3 GridCenter;
    [SerializeField]
    private Vector2 GridSize;    // 그리드 크기
    private int GridSizeX;      // 그리드 x 크기
    private int GridSizeY;      // 그리드 y 크기
    private readonly float Radius = 0.5f;

    private Coroutine CurrentCoroutine;

    private void Start()
    {
        TotalEliteNum = Random.Range(0, 3 + 1);
        if (ChanceMaker.GetThisChanceResult_Percentage(25))
            TotalGuvNum = 1;
        CreateGrid();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CurrentCoroutine == null)
            CurrentCoroutine = StartCoroutine(SponeEnemy());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CurrentCoroutine != null)
        {
            StopCoroutine(CurrentCoroutine);
            TotalEnemyNum = 0;
            CurrentCoroutine = null;
        }

    }

    private IEnumerator SponeEnemy()
    {
        while (TotalEnemyNum < 15 - DeathEnemyNum)
        {
            if (DeathEnemyNum >= 15)
                Destroy(gameObject);

            if (CurrentEnemyNum < 15)
            {
                GNode newStartPosition;
                do
                {
                    newStartPosition = Grid[Random.Range(0, GridSizeX), Random.Range(0, GridSizeY)];
                } while (newStartPosition.IsWall);

                PositioningEnemy(newStartPosition.WorldPos);

                TotalEnemyNum++;
                CurrentEnemyNum++;
                yield return new WaitForSeconds(0.01f);
            }
            else
                yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreateGrid()
    {
        GridCenter = transform.position;
        GridSizeX = Mathf.RoundToInt(GridSize.x);   // 그리드의 가로 크기
        GridSizeY = Mathf.RoundToInt(GridSize.y);   // 그리드의 세로 크기

        Grid = new GNode[GridSizeX, GridSizeY];
        Vector3 worldBottomLeft = GridCenter - Vector3.right * GridSize.x / 2 - Vector3.up * GridSizeY / 2;    // 현재 위치에서 왼쪽아래 좌표를 저장
        Vector3 worldPosition;
        for (int x = 0; x < GridSizeX; x++)
            for (int y = 0; y < GridSizeY; y++)
            {
                worldPosition = worldBottomLeft + Vector3.right * (x + Radius) + Vector3.up * (y + Radius);
                bool iswall = Physics2D.OverlapCircle(worldPosition, Radius - 0.1f, LayerMask.GetMask("Wall"));    // 해당 노드의 레이어 확인
                if (!iswall)
                    iswall = Physics2D.OverlapCircle(worldPosition, Radius - 0.1f, LayerMask.GetMask("Water"));    // 해당 노드의 레이어 확인

                Grid[x, y] = new GNode(iswall, worldPosition, x, y);
            }
    }

    private void PositioningEnemy(Vector3 newworldposition)
    {
        if(TotalGuvNum - DeathGuvNum > CurrnetGuvNum)
        {
            if (ChanceMaker.GetThisChanceResult_Percentage(50))
                MonsterPool.Instance.GetObject(MonsterPool.MonsterPrefabName.Kobold_Melee_Guv).PositioningEnemyBase(this, newworldposition);
            else
                MonsterPool.Instance.GetObject(MonsterPool.MonsterPrefabName.Kobold_Ranged_Guv).PositioningEnemyBase(this, newworldposition);
            
            CurrnetGuvNum += 1;
            return;
        }

        if(TotalEliteNum - DeathEliteNum > CurrnentEliteNum)
        {
            if (ChanceMaker.GetThisChanceResult_Percentage(50))
                MonsterPool.Instance.GetObject(MonsterPool.MonsterPrefabName.Kobold_Melee_Elite).PositioningEnemyBase(this, newworldposition);
            else
                MonsterPool.Instance.GetObject(MonsterPool.MonsterPrefabName.Kobold_Ranged_Elite).PositioningEnemyBase(this, newworldposition);

            CurrnentEliteNum += 1;
            return;
        }

        if (ChanceMaker.GetThisChanceResult_Percentage(50))
            MonsterPool.Instance.GetObject(MonsterPool.MonsterPrefabName.Kobold_Melee).PositioningEnemyBase(this, newworldposition);
        else
            MonsterPool.Instance.GetObject(MonsterPool.MonsterPrefabName.Kobold_Ranged).PositioningEnemyBase(this, newworldposition);
    }
}
