using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �޴� ��ư �����
[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 2)]
public class HealthPotion : ItemInfo_Consumable
{
    // ���Ǿ������� ȸ����
    [SerializeField]
    private int health;

    // ������ ����
    public void Use()
    {
        Player.MyInstance.NewBuff("HealPotion_Buff");
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: ü�� {0} ȸ��</color>", health);
    }
}
