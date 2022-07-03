using UnityEngine;

public abstract class ItemInfo_Base : ScriptableObject
{
    public enum Kinds { Common, Equipment, Potion }
    [SerializeField]
    private Kinds kind;// 아이템 종류
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
    private Sprite icon;    // 아이템 이미지
    public Sprite MyIcon { get { return icon; } }
    [HideInInspector]
    public SlotScript slot;

    public string itemName;   // 아이템 이름
    public string descript;// 아이템 설명 (배경설정같은것)
    public virtual string GetDescription() { return itemName; }
    public string effect;  // 아이템 효과 서술
    public int limitLevel; // 아이템 제한 레벨
}