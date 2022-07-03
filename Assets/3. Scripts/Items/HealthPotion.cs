using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상단 메뉴 버튼 만들기
[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 2)]
public class HealthPotion : ItemInfo_Consumable
{
    // 포션아이템의 회복량
    [SerializeField]
    private int health;

    // 아이템 사용시
    public void Use()
    {
        Player.MyInstance.NewBuff("HealPotion_Buff");
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: 체력 {0} 회복</color>", health);
    }
}
