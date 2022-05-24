using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Part { Helmet, Cloth, Pants, Weapon, Shoulder, Back }
// ��� �޴� ��ư �����
[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Items/Equipment", order = 3)]
public class EquipmentItem : Item
{
    [SerializeField]
    private Part part;
    public Part GetPart
    {
        get
        {
            return part;
        }
    }
    // ���Ǿ������� ȸ����
    [SerializeField]
    private int health;

    // ������ ����
    public void Equip()
    {

        Player.MyInstance.EquipItem(this);
    }





    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: ü�� {0} ȸ��</color>", health);
    }
}
