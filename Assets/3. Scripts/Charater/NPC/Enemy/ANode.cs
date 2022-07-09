using UnityEngine;

[System.Serializable]
public class ANode
{
    public bool isWW;         // 해당지점이 벽인지 아닌지
    public Vector3 worldPos;    // 월드 좌표
    public int GridX;           // 그리드 x좌표
    public int GridY;           // 그리드 y좌표

    public int gCost;           // 시작 노드부터 현재 노드까지의 비용
    public int hCost;           // 현재 노드에서 목표 노드까지의 비용

    public ANode parentNode;    // 부모 노드

    public ANode(bool nisWW, Vector3 nWorldPos, int nGridX, int nGridY)
    {
        isWW = nisWW;
        worldPos = nWorldPos;
        GridX = nGridX;
        GridY = nGridY;
    }

    public int fCost { get { return gCost + hCost; } }
}
