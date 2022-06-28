using UnityEngine;

public enum Kinds { Common, Equipment, Potion }

public abstract class ItemInfo : ScriptableObject
{
    [SerializeField]
    private Sprite icon;    // ������ �̹���
    [SerializeField]
    private int stackSize;  // ��ø ����
    public string itemName;   // ������ �̸�
    public string descript;// ������ ���� (��漳��������)
    public string effect;  // ������ ȿ�� ����
    public int limitLevel; // ������ ���� ����
    [SerializeField]
    private Kinds kind;// ������ ����
    [HideInInspector]
    public SlotScript slot;

    public Sprite MyIcon { get { return icon; } }
    // �������� ��ø�� �� �ִ� ����
    // ��) �Ҹ� ������ ��� �Ѱ��� Slot�� ��������
    //     ��ø�Ǿ ������ �� ����.
    public int MyStackSize { get { return stackSize; } }

    public Kinds GetKind
    {
        get
        {
            Kinds myKind = Kinds.Common;
            switch (kind)
            {
                case Kinds.Equipment:
                    myKind = Kinds.Equipment;
                    break;
                case Kinds.Potion:
                    myKind = Kinds.Potion;
                    break;
            }
            return myKind;
        }
    }

    public virtual string GetDescription()
    {
        return itemName;
    }
}