using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상단 메뉴 버튼 만들기
[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/HealthPotion")]
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
}
