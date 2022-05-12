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
    // 그리드 생성
    public LayerMask WallMask;

    private ANode[,] Grid;
    private Vector3 GridCenter;
    public Vector2 GridSize;    // 그리드 크기
    private int GridSizeX;      // 그리드 x 크기
    private int GridSizeY;      // 그리드 y 크기

    private readonly float nodeRadius = 0.5f;

    public List<ANode> path = new List<ANode>();    // 탐색완료 경로

    // 탐색
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
        GridSizeX = Mathf.RoundToInt(GridSize.x);   // 그리드의 가로 크기
        GridSizeY = Mathf.RoundToInt(GridSize.y);   // 그리드의 세로 크기

        Grid = new ANode[GridSizeX, GridSizeY];
        Vector3 worldBottomLeft = GridCenter - Vector3.right * GridSize.x / 2 - Vector3.up * GridSizeY / 2;    // 현재 위치에서 왼쪽아래 좌표를 저장
        Vector3 worldPosition;
        for (int x = 0; x < GridSizeX; x++)  // 
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                worldPosition = worldBottomLeft + Vector3.right * (x + nodeRadius) + Vector3.up * (y + nodeRadius);
                bool iswall = Physics2D.OverlapCircle(worldPosition, nodeRadius - 0.1f, WallMask);    // 해당 노드의 레이어 확인
                Grid[x, y] = new ANode(iswall, worldPosition, x, y);
            }
        }
    }

    public List<ANode> GetNeighbours(ANode node)    // 이웃 노드들을 검색한다.
    {
        List<ANode> neighobours = new List<ANode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // 자기 자신일 경우 스킵

                int CheckX = node.GridX + x;
                int CheckY = node.GridY + y;

                if (CheckX >= 0 && CheckX < GridSizeX && CheckY >= 0 && CheckY < GridSizeY)     // x, y의 값이 Grid 범위 안에 있을 경우
                    if (!Grid[node.GridX, CheckY].isWall && !Grid[CheckX, node.GridY].isWall)     // 벽 사이로 통과 안됨
                        if (!Grid[node.GridX, CheckX].isWall || !Grid[CheckX, node.GridY].isWall) // 코너를 가로질러 갈때 이동중 수직 수평 장애물이 있으면 안됨
                            neighobours.Add(Grid[CheckX, CheckY]);
            }
        }
        return neighobours;
    }

    public ANode GetNodeFromWorldPoint(Vector3 worldPosition)               // 노드의 월드 좌표찾기
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

            // 열린목록에서 Fcost가 가장 작은 노드를 찾는다. 만약 Fcost가 같다면 Hcout가 가장 작은 노드를 선택한다.
            for (int i = 1; i < openList.Count; i++)
                if (openList[i].fCost < CurrentNode.fCost || openList[i].fCost == CurrentNode.fCost && openList[i].hCost < CurrentNode.hCost)
                    CurrentNode = openList[i];

            // 탐색시작 탐색할 노드를 열린 목록에서 제외하고 닫힌목록에 추가한다.
            openList.Remove(CurrentNode);
            closedList.Add(CurrentNode);

            if (CurrentNode == targetNode)  // 현재 노드가 타겟노드일 경우 탐색을 종료 한다.
            {
                RetracePath(startNode, targetNode);
                break;
            }

            foreach (ANode n in GetNeighbours(CurrentNode))
            {
                if (n.isWall || closedList.Contains(n)) // 현재 노드가 벽일 경우 OR 닫힌목록에 포함되어 있는 경우 스킵
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
