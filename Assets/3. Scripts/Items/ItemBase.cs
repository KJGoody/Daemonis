using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quality { Normal, Advanced, Rare, Epic, Legendary, Relic }

public class ItemBase : IMoveable, IDescribable, IUseable
{
    public ItemInfo itemInfo;

    public Quality quality; // ������ ���

    public Sprite MyIcon // ������ �̹���
    {
        get { return itemInfo.MyIcon; }
    }
    
    // �������� ��ø�� �� �ִ� ����
    // ��) �Ҹ� ������ ��� �Ѱ��� Slot�� ��������
    //     ��ø�Ǿ ������ �� ����.
    public int MyStackSize { get { return itemInfo.MyStackSize; } }

    #region �������� ����
    public List<AddOption> addOptionList = new List<AddOption>();
        public void SetAddOption() // �߰��ɼ� ����
    {
        for (int i = 0; i < (int)(quality) + 1; i++)
        {
            int newTier = AddOptionManager.MyInstance.SetRandomTier(quality);
            int newOption = AddOptionManager.MyInstance.SetRandomKind();
            float newValue = 0;

            addOptionList.Add(new AddOption(newTier, newOption, newValue));
            newValue = AddOptionManager.MyInstance.SetRandomValue(addOptionList[i]);
            addOptionList[i].value = newValue;
        }
    }

    public Part GetPart
    {
        get
        {
            EquipmentItem equipmentItem = itemInfo as EquipmentItem;
            return equipmentItem.GetPart;
        }
    }

    private Sprite[] sprite; // ĳ���Ϳ� ����� ��� �̹���
    public Sprite[] itemSprite
    {
        get
        {
            EquipmentItem equipmentItem = itemInfo as EquipmentItem;
            return equipmentItem.itemSprite;
        }
    }

    public void ActiveEquipment(bool isActive) // ��� ���� & ����
    {
        EquipmentItem equipmentItem = itemInfo as EquipmentItem;
        equipmentItem.ActiveEquipment(isActive); // ��� ���̽� ���� ����
        if (addOptionList.Count > 0) // �߰� �ɼ� ����
        {
            for (int i = 0; i < addOptionList.Count; i++)
            {
                string optionName = AddOptionManager.MyInstance.GetOptionString(addOptionList[i].option_Num);
                float optionValue = addOptionList[i].value;
                if (optionName == "ItemLevel")
                {

                }
                else
                {
                    if (isActive) // �߰��ɼ� ��������
                        Player.MyInstance.Plus(optionName, optionValue);
                    else
                        Player.MyInstance.Plus(optionName, -optionValue);
                }
            }
        }

    }

    public float GetWeaponxDamage()
    {
        EquipmentItem equipmentItem = itemInfo as EquipmentItem;
        //return equipmentItem.WeaponxDamage;
        return 1;
    }
    #endregion

    public string MyName // ������ �̸� ǥ��
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
                case Quality.Legendary:
                    color = "#ffff00ff";
                    break;
                case Quality.Relic:
                    color = "#ff6600ff";
                    break;
            }

            return string.Format("<color={0}>{1}</color>", color, itemInfo.itemName);
        }

    }
    
    public string MyQualityText // ������ ��� ǥ��
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
                    color = "#ffff00ff";
                    str = "����";
                    break;
                case Quality.Relic:
                    color = "#ff6600ff";
                    str = "����";
                    break;
            }

            return string.Format("<color={0}>{1}</color>", color, str);

        }
    }
    public Kinds GetKind { get { return itemInfo.GetKind; } } // ������ ����
    public SlotScript MySlot 
    {
        get { return itemInfo.slot; }
        set { itemInfo.slot = value; }
    }
    public string MyDescript { get { return itemInfo.descript; } } // ������ ��漳��
    public string MyEffect { get { return itemInfo.effect; } } // ������ ȿ��
    public int MyLimitLevel { get { return itemInfo.limitLevel; } } // ������ ��� ���� ����

    public void Remove() // ����
    {
        if (MySlot != null)
        {
            //if (MySlot.MyCount == 0) // ���߿� �ٽ� �������� ���� ���� �����۽��� 0�϶� ���� �ϳ� ã�°�
            
            InventoryScript.MyInstance.FindUseSlot(this);
            MySlot.RemoveItem(this);
        }
    }
    public void EquipmentRemove() // �������� ����
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

        return string.Format("<color={0}>{1}</color>", color, itemInfo.itemName);
    }
    public void Use() // ���
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
