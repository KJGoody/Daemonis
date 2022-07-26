using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNode
{
    public enum NodeState { Floor, Wall, Water, None }
    public NodeState State;
    public Vector3 WorldPos;
    public int X;
    public int Y;

    public SNode(NodeState state, Vector3 worldPos, int x, int y)
    {
        State = state;
        WorldPos = worldPos;
        X = x;
        Y = y;
    }
}

public class EnemySpawn : MonoBehaviour
{
    private SNode[,] Grid;
    private Vector3 GridCenter;
    [SerializeField] private Vector2 GridSize;
    private int Gridx;
    private int Gridy;
    private readonly float Radius = 0.5f;

    private void CreateGrid()
    {
        GridCenter = transform.position;
        Gridx = Mathf.RoundToInt(GridSize.x);   // 그리드의 가로 크기
        Gridy = Mathf.RoundToInt(GridSize.y);   // 그리드의 세로 크기

        Grid = new SNode[Gridx, Gridy];
        Vector3 worldBottomLeft = GridCenter - Vector3.right * GridSize.x / 2 - Vector3.up * Gridy / 2;    // 현재 위치에서 왼쪽아래 좌표를 저장
        for (int x = 0; x < Gridx; x++)
            for (int y = 0; y < Gridy; y++)
            {
                Vector3 worldPosition = worldBottomLeft + Vector3.right * (x + Radius) + Vector3.up * (y + Radius);
                Grid[x, y] = new SNode(CheckNodeState(worldPosition), worldPosition, x, y);
            }
    }

    private SNode.NodeState CheckNodeState(Vector3 WorldPosition)
    {
        if (Physics2D.OverlapCircle(WorldPosition, Radius - 0.1f, LayerMask.GetMask("Floor")))
            return SNode.NodeState.Floor;

        if (Physics2D.OverlapCircle(WorldPosition, Radius - 0.1f, LayerMask.GetMask("Wall")))
            return SNode.NodeState.Wall;

        if (Physics2D.OverlapCircle(WorldPosition, Radius - 0.1f, LayerMask.GetMask("Water")))
            return SNode.NodeState.Water;

        return SNode.NodeState.None;
    }

    private void Start()
    {
        CreateGrid();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(GridSize.x, GridSize.y));
    //    if (Grid != null)
    //    {
    //        foreach (ANode n in Grid)
    //        {
    //            Gizmos.color = (n.isWall) ? Color.red : Color.white;

    //            if (path != null)
    //            {
    //                if (path.Contains(n))
    //                    Gizmos.color = Color.black;
    //            }
    //            Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeRadius * 2 - 0.1f));
    //        }
    //    }
    //}
}
