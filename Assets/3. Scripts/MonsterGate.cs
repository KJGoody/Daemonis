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
    [SerializeField]
    private int TotalEnemyNum;
    [HideInInspector]
    public int CurrentEnemyNum;

    private GNode[,] Grid;
    private Vector3 GridCenter;
    [SerializeField]
    private  Vector2 GridSize;    // �׸��� ũ��
    private int GridSizeX;      // �׸��� x ũ��
    private int GridSizeY;      // �׸��� y ũ��
    private readonly float Radius = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CreateGrid();
            StartCoroutine(SponeEnemy());
        }
    }

    private IEnumerator SponeEnemy()
    {
        while (TotalEnemyNum < 100)
        {
            if (CurrentEnemyNum < 2)
            {
                GNode newStartPosition;
                do
                {
                    newStartPosition = Grid[Random.Range(0, GridSizeX), Random.Range(0, GridSizeY)];
                } while (newStartPosition.IsWall);

                MonsterPool.Instance.GetObject().PositioningEnemyBase(this, newStartPosition.WorldPos);
                TotalEnemyNum++;
                CurrentEnemyNum++;
                //yield return new WaitForSeconds(Random.value);
                yield return new WaitForSeconds(0.01f);
            }
            else
                yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    private void CreateGrid()
    {
        GridCenter = transform.position;
        GridSizeX = Mathf.RoundToInt(GridSize.x);   // �׸����� ���� ũ��
        GridSizeY = Mathf.RoundToInt(GridSize.y);   // �׸����� ���� ũ��

        Grid = new GNode[GridSizeX, GridSizeY];
        Vector3 worldBottomLeft = GridCenter - Vector3.right * GridSize.x / 2 - Vector3.up * GridSizeY / 2;    // ���� ��ġ���� ���ʾƷ� ��ǥ�� ����
        Vector3 worldPosition;
        for (int x = 0; x < GridSizeX; x++)  // 
            for (int y = 0; y < GridSizeY; y++)
            {
                worldPosition = worldBottomLeft + Vector3.right * (x + Radius) + Vector3.up * (y + Radius);
                bool iswall = Physics2D.OverlapCircle(worldPosition, Radius - 0.1f, LayerMask.GetMask("Wall"));    // �ش� ����� ���̾� Ȯ��
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
