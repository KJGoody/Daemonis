using UnityEngine;

// 상단 메뉴 버튼 만들기
public class ItemInfo_Equipment : ItemInfo_Base
{
    public enum Parts { Back, Cloth, Helmet, Shoes, Shoulder, Weapon }
    public Parts Part;
    public string ItemSprite;
    public string BaseOption;
    public int BaseOptionValue;
}
