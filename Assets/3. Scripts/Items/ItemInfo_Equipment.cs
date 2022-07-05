using UnityEngine;

// ��� �޴� ��ư �����
[CreateAssetMenu(fileName = "Item_Equipment", menuName = "Items/Item_Equipment", order = 3)]
public class ItemInfo_Equipment : ItemInfo_Base
{
    public enum Part { Helmet, Cloth, Shoes, Weapon, Shoulder, Back }
    [SerializeField]
    private Part part;
    public Part GetPart { get { return part; } }

    [SerializeField]
    private Sprite[] ItemSprite;
    public Sprite[] GetItemSprite { get { return ItemSprite; } }
    [SerializeField]
    private string BaseOption;
    [SerializeField]
    private float BaseOptionValue;

    public void ActiveEquipmentStat(bool isActive)  // ��� ���� ��, ���� �� ������ ��ȭ ��Ű�� �Լ�
    {
        if (isActive)
            Player.MyInstance.PlusStat(BaseOption, BaseOptionValue);
        else
            Player.MyInstance.PlusStat(BaseOption, -BaseOptionValue);
    }

    public int GetWeaponxDamage()
    {
        return 1;
    }
}
