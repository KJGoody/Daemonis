using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �޴� ��ư �����
[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 2)]
public class HealthPotion : Item, IUseable
{
    // ���Ǿ������� ȸ����
    [SerializeField]
    private int health;

    // ������ ����
    public void Use()
    {
        // ü���� �ִ�ü�º��� ������
        if (Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            // ����ϴ� �������� ���ְ�
            Remove();

            // ü���� ȸ���Ѵ�.
            Player.MyInstance.MyHealth.MyCurrentValue += health;
        }
    }
}
