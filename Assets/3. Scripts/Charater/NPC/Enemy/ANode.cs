using UnityEngine;

[System.Serializable]
public class ANode
{
    public bool isWW;         // �ش������� ������ �ƴ���
    public Vector3 worldPos;    // ���� ��ǥ
    public int GridX;           // �׸��� x��ǥ
    public int GridY;           // �׸��� y��ǥ

    public int gCost;           // ���� ������ ���� �������� ���
    public int hCost;           // ���� ��忡�� ��ǥ �������� ���

    public ANode parentNode;    // �θ� ���

    public ANode(bool nisWW, Vector3 nWorldPos, int nGridX, int nGridY)
    {
        isWW = nisWW;
        worldPos = nWorldPos;
        GridX = nGridX;
        GridY = nGridY;
    }

    public int fCost { get { return gCost + hCost; } }
}
