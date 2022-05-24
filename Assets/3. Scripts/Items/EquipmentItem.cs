using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Part { Helmet, Cloth, Shoes, Weapon, Shoulder, Back }
// 상단 메뉴 버튼 만들기
[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Items/Equipment", order = 3)]
public class EquipmentItem : Item
{
    [SerializeField]
    private Part part;
    public Part GetPart
    {
        get
        {
            return part;
        }
    }
    [SerializeField]
    private Sprite[] sprite;
    public Sprite[] itemSprite
    {
        get
        {
            return sprite;
        }
    }
    
    // 아이템 사용시
    public void Equip()
    {

        Player.MyInstance.EquipItem(this);
    }





    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: 체력 {0} 회복</color>");
    }
}
