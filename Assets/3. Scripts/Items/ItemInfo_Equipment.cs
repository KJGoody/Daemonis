using UnityEngine;

// ��� �޴� ��ư �����
public class ItemInfo_Equipment : ItemInfo_Base
{
    public enum Parts { Back, Cloth, Helmet, Shoes, Shoulder, Weapon }
    public Parts Part;
    public string ItemSprite;
    public string BaseOption;
    public int BaseOptionValue;
}
