using UnityEngine;

// ��� �޴� ��ư �����
public class ItemInfo_Equipment : ItemInfo_Base
{
    public enum Part { Back, Cloth, Helmet, Shoes, Shoulder, Weapon }
    public Part part;
    public string ItemSprite;
    public string BaseOption;
    public int BaseOptionValue;
}
