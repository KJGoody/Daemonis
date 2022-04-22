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
        if (Player.MyInstance.MyHealth.StatBarCurrentValue < Player.MyInstance.MyHealth.StatBarMaxValue)
        {
            // ����ϴ� �������� ���ְ�

            Remove();

            // ü���� ȸ���Ѵ�.
            Player.MyInstance.MyHealth.StatBarCurrentValue += health;
        }
    }
    public string GetName()
    {
        return MyName;
    }
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: ü�� {0} ȸ��</color>", health);
    }
}
