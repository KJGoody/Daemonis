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

    private GNode[,] Grid;
    private Vector3 GridCenter;
    [SerializeField]
    private  Vector2 GridSize;    // �׸��� ũ��
    private int GridSizeX;      // �׸��� x ũ��
    private int GridSizeY;      // �׸��� y ũ��
    private readonly float Radius = 0.5f;

    private Coroutine CurrentCoroutine;

    private void Start()
    {
        CreateGrid();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CurrentCoroutine == null)
            CurrentCoroutine = StartCoroutine(SponeEnemy());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && CurrentCoroutine != null)
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

                if (ChanceMaker.GetThisChanceResult_Percentage(50))
                    MonsterPool.Instance.GetObject(MonsterPool.MonsterPrefabName.Kobold_Melee).PositioningEnemyBase(this, newStartPosition.WorldPos);
                else
                    MonsterPool.Instance.GetObject(MonsterPool.MonsterPrefabName.Kobold_Ranged).PositioningEnemyBase(this, newStartPosition.WorldPos);

                TotalEnemyNum++;
                CurrentEnemyNum++;
                //yield return new WaitForSeconds(Random.value);
                yield return new WaitForSeconds(0.01f);
            }
            else
                yield return new WaitForSeconds(0.1f);
        }
    }

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
                bool iswall = Physics2D.OverlapCircle(worldPosition, Radius - 0.1f, LayerMask.GetMask("Wall"));    // �ش� ����� ���̾� Ȯ��
                if(!iswall)
                    iswall = Physics2D.OverlapCircle(worldPosition, Radius - 0.1f, LayerMask.GetMask("Water"));    // �ش� ����� ���̾� Ȯ��

                Grid[x, y] = new GNode(iswall, worldPosition, x, y);
            }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(GridSize.x, GridSize.y));
    //    if (Grid != null)
    //    {
    //        foreach (GNode n in Grid)
    //        {
    //            Gizmos.color = (n.IsWall) ? Color.red : Color.white;
    //            Gizmos.DrawCube(n.WorldPos, Vector3.one * (Radius * 2 - 0.1f));
    //        }
    //    }
    //}
}
