using UnityEngine;

public abstract class ItemInfo_Base : ScriptableObject
{
    public enum Kinds { Common, Equipment, Potion }
    [SerializeField]
    private Kinds kind;// ������ ����
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

    [SerializeField]
    private Sprite icon;    // ������ �̹���
    public Sprite MyIcon { get { return icon; } }
    [HideInInspector]
    public SlotScript slot;

    public string itemName;   // ������ �̸�
    public string descript;// ������ ���� (��漳��������)
    public virtual string GetDescription() { return itemName; }
    public string effect;  // ������ ȿ�� ����
    public int limitLevel; // ������ ���� ����
}