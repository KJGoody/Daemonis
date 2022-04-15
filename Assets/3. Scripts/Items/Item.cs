using UnityEngine;

public enum Quality { Normal, Advanced, Rare, Epic , Legendary , Relic }
public enum Kinds { Common, Equipment, Potion }

public abstract class Item : ScriptableObject, IMoveable , IDescribable
{
    [SerializeField]
    private Sprite icon;    // 아이템 이미지
    [SerializeField]
    private int stackSize;  // 중첩 스택
    [SerializeField]
    private string itemName;   // 아이템 이름
    [SerializeField]
    private Quality quality;// 아이템 등급
    [SerializeField]
    private string descript;// 아이템 설명 (배경설정같은것)
    [SerializeField]
    private string effect;  // 아이템 효과 서술
    [SerializeField]
    private int limitLevel; // 아이템 제한 레벨
    [SerializeField]
    private Kinds kind;// 아이템 종류
    private SlotScript slot;


    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }
    // 아이템이 중첩될 수 있는 개수
    // 예) 소모성 물약의 경우 한개의 Slot에 여러개가
    //     중첩되어서 보관될 수 있음.
    public int MyStackSize
    {
        get
        {
            return stackSize;
        }
    }
    public string MyName
    {
        get
        {
            string color = string.Empty;

            switch (quality)
            {
                case Quality.Normal:
                    color = "#d6d6d6";
                    break;
                case Quality.Advanced:
                    color = "#00ff00ff";
                    break;
                case Quality.Rare:
                    color = "#0000ffff";
                    break;
                case Quality.Epic:
                    color = "#800080ff";
                    break;
            }

            return string.Format("<color={0}>{1}</color>", color, itemName);
        }

        set
        {
            itemName = value;
        }
    }
    public string MyQuality
    {
        get
        {
            string color = string.Empty;
            string str="";
            switch (quality)
            {
                case Quality.Normal:
                    color = "#d6d6d6";
                    str = "노말";
                    break;
                case Quality.Advanced:
                    color = "#00ff00ff";
                    str = "고급";
                    break;
                case Quality.Rare:
                    color = "#0000ffff";
                    str = "희귀";
                    break;
                case Quality.Epic:
                    color = "#800080ff";
                    str = "영웅";
                    break;
            }

            return string.Format("<color={0}>{1}</color>", color, str);
        }

        set
        {
            itemName = value;
        }
    }
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
    public SlotScript MySlot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }
    public string MyDescript
    {
        get
        {
            return descript;
        }

        set
        {
            descript = value;
        }
    }
    public string MyEffect
    {
        get
        {
            return effect;
        }

        set
        {
            effect = value;
        }
    }
    public int MyLimitLevel
    {
        get
        {
            return limitLevel;
        }

        set
        {
            limitLevel = value;
        }
    }
    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
    public virtual string GetDescription()
    {
        string color = string.Empty;

        switch (quality)
        {
            case Quality.Normal:
                color = "#d6d6d6";
                break;
            case Quality.Advanced:
                color = "#00ff00ff";
                break;
            case Quality.Rare:
                color = "#0000ffff";
                break;
            case Quality.Epic:
                color = "#800080ff";
                break;
        }

        return string.Format("<color={0}>{1}</color>", color, itemName);
    }


}