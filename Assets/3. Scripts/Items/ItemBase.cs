using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Quality { Normal, Advanced, Rare, Epic, Legendary, Relic }
public class ItemBase : IMoveable, IDescribable, IUseable
{
    public Item itemInfo;
    [SerializeField]
    private Quality quality;// 아이템 등급

    public Quality MyQuality
    {
        get
        {
            return quality;
        }
        set
        {
            quality = value;
        }
    }
    public Sprite MyIcon
    {
        get
        {
            return itemInfo.MyIcon;
        }
    }
    // 아이템이 중첩될 수 있는 개수
    // 예) 소모성 물약의 경우 한개의 Slot에 여러개가
    //     중첩되어서 보관될 수 있음.
    public int MyStackSize
    {
        get
        {
            return itemInfo.MyStackSize;
        }
    }

    #region 장비아이템 관련
    private Part part;
    public Part GetPart
    {
        get
        {
            EquipmentItem equipmentItem = itemInfo as EquipmentItem;
            return equipmentItem.GetPart;
        }
    }
    private Sprite[] sprite;
    public Sprite[] itemSprite
    {
        get
        {
            EquipmentItem equipmentItem = itemInfo as EquipmentItem;
            return equipmentItem.itemSprite;
        }
    }
    public void ActiveEquipment(bool isActive)
    {
        EquipmentItem equipmentItem = itemInfo as EquipmentItem;
        equipmentItem.ActiveEquipment(isActive);
    }
    #endregion

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

            return string.Format("<color={0}>{1}</color>", color, itemInfo.MyName);
        }

    }
    public string MyQualityText
    {
        get
        {
            string color = string.Empty;
            string str = "";
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
    }
    public Kinds GetKind
    {
        get
        {
            return itemInfo.GetKind;
        }
    }
    public SlotScript MySlot
    {
        get
        {
            return itemInfo.MySlot;
        }
        set
        {
            itemInfo.MySlot = value;
        }
    }
    public string MyDescript
    {
        get
        {
            return itemInfo.MyDescript;
        }
    }
    public string MyEffect
    {
        get
        {
            return itemInfo.MyEffect;
        }
    }
    public int MyLimitLevel
    {
        get
        {
            return itemInfo.MyLimitLevel;
        }
    }
    public void Remove()
    {
        if (MySlot != null)
        {
            //if (MySlot.MyCount == 0) // 나중에 다시 넣을수도 있음 포션 아이템슬롯 0일때 새로 하나 찾는거
                InventoryScript.MyInstance.FindUseSlot(this);
            MySlot.RemoveItem(this);
        }
    }
    public void EquipmentRemove()
    {
        InventoryScript.MyInstance.FindEquipment(this);
        MySlot.RemoveItem(this);
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

        return string.Format("<color={0}>{1}</color>", color, itemInfo.MyName);
    }
    public void Use()
    {
        if(this.GetKind == Kinds.Potion)
        {
            HealthPotion healthPotion = itemInfo as HealthPotion;
            healthPotion.Use();
            Remove();
        }
        else if(this.GetKind == Kinds.Equipment)
        {
            EquipmentItem equipmentItem = itemInfo as EquipmentItem;
            Player.MyInstance.EquipItem(this);
            EquipmentRemove();
        }
    }
    public string GetName() // 일단 useable때문에 넣어두긴함
    {
        return MyName;
    }

}
