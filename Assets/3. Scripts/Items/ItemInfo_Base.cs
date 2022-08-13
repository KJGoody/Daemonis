using UnityEngine;

public abstract class ItemInfo_Base
{
    public string ID;
    public enum Kinds { Common, Potion, Equipment }
    public Kinds Kind;
    public Sprite Icon;     // 아이템 이미지
    public string Name;     // 아이템 이름
    public string Descript; // 아이템 설명 (배경설정같은것)
    public string Effect;   // 아이템 효과 서술
    public int LimitLevel;  // 아이템 제한 레벨
    public int Cost;        // 가격
}