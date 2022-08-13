using UnityEngine;

// 상단 메뉴 버튼 만들기
public class ItemInfo_Equipment : ItemInfo_Base
{
    public enum Part { Back, Cloth, Helmet, Shoes, Shoulder, Weapon }
    public Part part;
    public string ItemSprite;
    public string BaseOption;
    public int BaseOptionValue;
}
