using UnityEngine;

// 상단 메뉴 버튼 만들기
public class ItemInfo_Equipment : ItemInfo_Base
{
    public enum Parts { Helmet, Cloth, Shoes, Weapon, Shoulder, Back }
    public Parts Part;
    public string ItemSprite;
    public string BaseOption;
    public int BaseOptionValue;
}
