using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �޴� ��ư �����
[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/HealthPotion")]
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
}
