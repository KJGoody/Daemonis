using UnityEngine;

// ��� �޴� ��ư �����
[CreateAssetMenu(fileName = "Item_Equipment", menuName = "Items/Item_Equipment")]
public class ItemInfo_Equipment : ItemInfo_Base
{
    public enum Part { Helmet, Cloth, Shoes, Weapon, Shoulder, Back }
    public Part part;
    public Sprite[] ItemSprite;
    public string BaseOption;
    public float BaseOptionValue;
}
