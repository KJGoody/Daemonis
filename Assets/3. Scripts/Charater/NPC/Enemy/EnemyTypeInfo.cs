using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeInfo
{
    public string ID;
    public string Name;
    public string Sound;
    public enum AttackTypes
    {
        #region 생략
        BaseMelee,
        BaseRanged,
        BaseAOE,
        Kobold_Ranged
        #endregion
    }
    public AttackTypes AttackType;
    public float AttackRange; // 공격 사거리
    public float AttackDelay; // 공격 딜레이
    public int EXP;
}
