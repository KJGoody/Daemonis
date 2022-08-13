using UnityEngine;

public abstract class ItemInfo_Base
{
    public string ID;
    public enum Kinds { Common, Potion, Equipment }
    public Kinds Kind;
    public Sprite Icon;     // ������ �̹���
    public string Name;     // ������ �̸�
    public string Descript; // ������ ���� (��漳��������)
    public string Effect;   // ������ ȿ�� ����
    public int LimitLevel;  // ������ ���� ����
    public int Cost;        // ����
}