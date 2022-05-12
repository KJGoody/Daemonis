using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANode
{
    public bool isWall;
    public Vector3 worldPos;
    public int GridX;
    public int GridY;

    public int gCost;
    public int hCost;

    public ANode parentNode;


    public ANode(bool nisWall, Vector3 nWorldPos, int nGridX, int nGridY)
    {
        isWall = nisWall;
        worldPos = nWorldPos;
        GridX = nGridX;
        GridY = nGridY;
    }

    public int fCost { get { return gCost + hCost; } }
}

public class ANav : MonoBehaviour
{
    // �׸��� ����
    public LayerMask WallMask;

    private ANode[,] Grid;
    private Vector3 GridCenter;
    public Vector2 GridSize;    // �׸��� ũ��
    private int GridSizeX;      // �׸��� x ũ��
    private int GridSizeY;      // �׸��� y ũ��

    private readonly float nodeRadius = 0.5f;

    public List<ANode> path = new List<ANode>();    // Ž���Ϸ� ���

    // Ž��
    private EnemyBase parent;

    private Vector3 StartPoint;
    private Vector3 TargetPoint;

    public bool EndPathFinding = false;
    public int CurrentPathNode;

    private void Awake()
    {
        parent = GetComponentInParent<EnemyBase>();

        CreateGrid();

        StartPoint = parent.transform.position;
        TargetPoint = parent.myStartPosition;
    }

    private void Start()
    {
        FindPath(StartPoint, TargetPoint);
    }

    private void CreateGrid()
    {
        GridCenter = new Vector3(Mathf.Floor(parent.transform.position.x), Mathf.Floor(parent.transform.position.y));
        GridSizeX = Mathf.RoundToInt(GridSize.x);   // �׸����� ���� ũ��
        GridSizeY = Mathf.RoundToInt(GridSize.y);   // �׸����� ���� ũ��

        Grid = new ANode[GridSizeX, GridSizeY];
        Vector3 worldBottomLeft = GridCenter - Vector3.right * GridSize.x / 2 - Vector3.up * GridSizeY / 2;    // ���� ��ġ���� ���ʾƷ� ��ǥ�� ����
        Vector3 worldPosition;
        for (int x = 0; x < GridSizeX; x++)  // 
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                worldPosition = worldBottomLeft + Vector3.right * (x + nodeRadius) + Vector3.up * (y + nodeRadius);
                bool iswall = Physics2D.OverlapCircle(worldPosition, nodeRadius - 0.1f, WallMask);    // �ش� ����� ���̾� Ȯ��
                Grid[x, y] = new ANode(iswall, worldPosition, x, y);
            }
        }
    }

    public List<ANode> GetNeighbours(ANode node)    // �̿� ������ �˻��Ѵ�.
    {
        List<ANode> neighobours = new List<ANode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // �ڱ� �ڽ��� ��� ��ŵ

                int CheckX = node.GridX + x;
                int CheckY = node.GridY + y;

                if (CheckX >= 0 && CheckX < GridSizeX && CheckY >= 0 && CheckY < GridSizeY)     // x, y�� ���� Grid ���� �ȿ� ���� ���
                    if (!Grid[node.GridX, CheckY].isWall && !Grid[CheckX, node.GridY].isWall)     // �� ���̷� ��� �ȵ�
                        if (!Grid[node.GridX, CheckX].isWall || !Grid[CheckX, node.GridY].isWall) // �ڳʸ� �������� ���� �̵��� ���� ���� ��ֹ��� ������ �ȵ�
                            neighobours.Add(Grid[CheckX, CheckY]);
            }
        }
        return neighobours;
    }

    public ANode GetNodeFromWorldPoint(Vector3 worldPosition)               // ����� ���� ��ǥã��
    {
        float percentX = Mathf.Clamp01((worldPosition.x - (GridCenter.x - GridSize.x / 2)) / GridSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y - (GridCenter.y - GridSize.y / 2) + 0.3f) / GridSize.y);

        int x = Mathf.RoundToInt((GridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((GridSizeY - 1) * percentY);

        return Grid[x, y];
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        ANode startNode = GetNodeFromWorldPoint(startPos);
        ANode targetNode = GetNodeFromWorldPoint(targetPos);

        List<ANode> openList = new List<ANode>();
        HashSet<ANode> closedList = new HashSet<ANode>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            ANode CurrentNode = openList[0];

            // ������Ͽ��� Fcost�� ���� ���� ��带 ã�´�. ���� Fcost�� ���ٸ� Hcout�� ���� ���� ��带 �����Ѵ�.
            for (int i = 1; i < openList.Count; i++)
                if (openList[i].fCost < CurrentNode.fCost || openList[i].fCost == CurrentNode.fCost && openList[i].hCost < CurrentNode.hCost)
                    CurrentNode = openList[i];

            // Ž������ Ž���� ��带 ���� ��Ͽ��� �����ϰ� ������Ͽ� �߰��Ѵ�.
            openList.Remove(CurrentNode);
            closedList.Add(CurrentNode);

            if (CurrentNode == targetNode)  // ���� ��尡 Ÿ�ٳ���� ��� Ž���� ���� �Ѵ�.
            {
                RetracePath(startNode, targetNode);
                break;
            }

            foreach (ANode n in GetNeighbours(CurrentNode))
            {
                if (n.isWall || closedList.Contains(n)) // ���� ��尡 ���� ��� OR ������Ͽ� ���ԵǾ� �ִ� ��� ��ŵ
                    continue;

                int newCurrentToNeightbourCost = CurrentNode.gCost + GetDistanceCost(CurrentNode, n);
                if (newCurrentToNeightbourCost < n.gCost || !openList.Contains(n))
                {
                    n.gCost = newCurrentToNeightbourCost;
                    n.hCost = GetDistanceCost(n, targetNode);
                    n.parentNode = CurrentNode;

                    if (!openList.Contains(n))
                        openList.Add(n);
                }
            }
        }

        EndPathFinding = true;
        CurrentPathNode = path.Count - 1;
    }

    void RetracePath(ANode startNode, ANode endNode)
    {
        ANode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
    }

    int GetDistanceCost(ANode nodeA, ANode nodeB)
    {
        int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    public void DestroyANav()
    {
        Destroy(gameObject);
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
