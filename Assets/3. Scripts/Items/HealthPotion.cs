using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상단 메뉴 버튼 만들기
[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 2)]
public class HealthPotion : Item, IUseable
{
    // 포션아이템의 회복량
    [SerializeField]
    private int health;

    // 아이템 사용시
    public void Use()
    {
        // 체력이 최대체력보다 낮으면
        if (Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            // 사용하는 아이템을 없애고
            Remove();

            // 체력을 회복한다.
            Player.MyInstance.MyHealth.MyCurrentValue += health;
        }
    }
}
