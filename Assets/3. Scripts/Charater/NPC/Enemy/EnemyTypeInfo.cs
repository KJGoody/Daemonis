using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeInfo
{
    public string StageNum;
    public string Name;
    public GameObject Prefab;
    public string Sound;
    public enum AttackTypes
    {
        #region 생략
        BaseMelee,
        BaseRanged,
        BaseRush,
        BaseAOE,
        Kobold_Ranged,
        Dwarf_Ranged,
        Siren_Ranged
        #endregion
    }
    public AttackTypes AttackType;
    public float AttackRange; // 공격 사거리
    public float AttackDelay; // 공격 딜레이
    public int EXP;
    //--Stat--
    public int Level;
    public int Attack;
    public int MaxHealth;
    public int MoveSpeed;
    public int HitPercent;
}
