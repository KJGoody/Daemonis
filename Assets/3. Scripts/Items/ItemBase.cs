using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Quality { Normal, Advanced, Rare, Epic, Legendary, Relic }
public class ItemBase : IMoveable, IDescribable, IUseable
{
    public Item itemInfo;
    [SerializeField]
    private Quality quality;// ������ ���

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
    // �������� ��ø�� �� �ִ� ����
    // ��) �Ҹ� ������ ��� �Ѱ��� Slot�� ��������
    //     ��ø�Ǿ ������ �� ����.
    public int MyStackSize
    {
        get
        {
            return itemInfo.MyStackSize;
        }
    }

    #region �������� ����
    public List<AddOption> addOptionList = new List<AddOption>();
    public void SetAddOption()
    {
        //if (addOptionList[0] != null)
        //    addOptionList.Clear();
        Debug.LogWarning("NewItem, Quality = " + quality);
        for (int i = 0; i < (int)(quality)+1; i++)
        {
            int newTier = AddOptionManager.MyInstance.SetRandomTier(quality);
            int newOption = AddOptionManager.MyInstance.SetRandomKind();
            float newValue = 0;
            addOptionList.Add(new AddOption(newTier, newOption, newValue));
            Debug.Log(i);
            newValue =    AddOptionManager.MyInstance.SetRandomValue(addOptionList[i]);
            Debug.Log(newValue);
            addOptionList[i].value = newValue; 
            
        }
        
    }
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
    public float GetWeaponxDamage()
    {
        EquipmentItem equipmentItem = itemInfo as EquipmentItem;
        //return equipmentItem.WeaponxDamage;
        return 1;
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
                    str = "�븻";
                    break;
                case Quality.Advanced:
                    color = "#00ff00ff";
                    str = "���";
                    break;
                case Quality.Rare:
                    color = "#0000ffff";
                    str = "���";
                    break;
                case Quality.Epic:
                    color = "#800080ff";
                    str = "����";
                    break;
                case Quality.Legendary:
                    color = "#800080ff";
                    str = "����";
                    break;
                case Quality.Relic:
                    color = "#800080ff";
                    str = "����";
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
            //if (MySlot.MyCount == 0) // ���߿� �ٽ� �������� ���� ���� �����۽��� 0�϶� ���� �ϳ� ã�°�
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
    public string GetName() // �ϴ� useable������ �־�α���
    {
        return MyName;
    }

}
