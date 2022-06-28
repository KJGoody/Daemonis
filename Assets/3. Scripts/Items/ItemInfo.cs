using UnityEngine;

public enum Kinds { Common, Equipment, Potion }

public abstract class ItemInfo : ScriptableObject
{
    [SerializeField]
    private Sprite icon;    // 아이템 이미지
    [SerializeField]
    private int stackSize;  // 중첩 스택
    public string itemName;   // 아이템 이름
    public string descript;// 아이템 설명 (배경설정같은것)
    public string effect;  // 아이템 효과 서술
    public int limitLevel; // 아이템 제한 레벨
    [SerializeField]
    private Kinds kind;// 아이템 종류
    [HideInInspector]
    public SlotScript slot;

    public Sprite MyIcon { get { return icon; } }
    // 아이템이 중첩될 수 있는 개수
    // 예) 소모성 물약의 경우 한개의 Slot에 여러개가
    //     중첩되어서 보관될 수 있음.
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