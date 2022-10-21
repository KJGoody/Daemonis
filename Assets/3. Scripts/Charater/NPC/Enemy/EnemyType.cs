using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public enum EnemyTypes
    {
        #region 생략
        Enemy_Koblod_Melee_Normal,
        Enemy_Kobold_Melee_Elite,
        Enemy_Kobold_Melee_Guv,
        Enemy_Kobold_Ranged_Normal,
        Enemy_Kobold_Ranged_Elite,
        Enemy_Kobold_Ranged_Guv,
        Enemy_Kobold_Rush_Normal,
        Enemy_Kobold_Rush_Elite,
        Enemy_Kobold_Rush_Guv,
        Enemy_Kobold_Melee_Boss,
        Enemy_Kobold_Rnaged_Boss,
        Enemy_Kobold_Rush_Boss,
        Enemy_Dwarf_Melee_Normal,
        Enemy_Dwarf_Melee_Elite,
        Enemy_Dwarf_Melee_Guv,
        Enemy_Dwarf_Ranged_Normal,
        Enemy_Dwarf_Ranged_Elite,
        Enemy_Dwarf_Ranged_Guv,
        Enemy_Dwarf_Rush_Normal,
        Enemy_Dwarf_Rush_Elite,
        Enemy_Dwarf_Rush_Guv,
        Enemy_Dwarf_Melee_Boss,
        Enemy_Dwarf_Rnaged_Boss,
        Enemy_Dwarf_Rush_Boss

        #endregion
    }
    public EnemyTypes enemyType;
    public string strEnemyType
    {
        get
        {
            switch (enemyType)
            {
                #region 생략
                //-- Act1 --
                case EnemyTypes.Enemy_Koblod_Melee_Normal:
                    return "P_Kobold_Melee_Normal";
                case EnemyTypes.Enemy_Kobold_Melee_Elite:
                    return "P_Kobold_Melee_Elite";
                case EnemyTypes.Enemy_Kobold_Melee_Guv:
                    return "P_Kobold_Melee_Guv";
                case EnemyTypes.Enemy_Kobold_Ranged_Normal:
                    return "P_Kobold_Ranged_Normal";
                case EnemyTypes.Enemy_Kobold_Ranged_Elite:
                    return "P_Kobold_Ranged_Elite";
                case EnemyTypes.Enemy_Kobold_Ranged_Guv:
                    return "P_Kobold_Ranged_Guv";
                case EnemyTypes.Enemy_Kobold_Rush_Normal:
                    return "P_Kobold_Rush_Normal";
                case EnemyTypes.Enemy_Kobold_Rush_Elite:
                    return "P_Kobold_Rush_Elite";
                case EnemyTypes.Enemy_Kobold_Rush_Guv:
                    return "P_Kobold_Rush_Guv";

                case EnemyTypes.Enemy_Kobold_Melee_Boss:
                    return "P_Kobold_Melee_Boss";
                case EnemyTypes.Enemy_Kobold_Rnaged_Boss:
                    return "P_Kobold_Ranged_Boss";
                case EnemyTypes.Enemy_Kobold_Rush_Boss:
                    return "P_Kobold_Rush_Boss";
                //-- Act1 --

                //-- Act2 --
                case EnemyTypes.Enemy_Dwarf_Melee_Normal:
                    return "P_Dwarf_Melee_Normal";
                case EnemyTypes.Enemy_Dwarf_Melee_Elite:
                    return "P_Dwarf_Melee_Elite";
                case EnemyTypes.Enemy_Dwarf_Melee_Guv:
                    return "P_Dwarf_Melee_Guv";
                case EnemyTypes.Enemy_Dwarf_Ranged_Normal:
                    return "P_Dwarf_Ranged_Normal";
                case EnemyTypes.Enemy_Dwarf_Ranged_Elite:
                    return "P_Dwarf_Ranged_Elite";
                case EnemyTypes.Enemy_Dwarf_Ranged_Guv:
                    return "P_Dwarf_Ranged_Guv";
                case EnemyTypes.Enemy_Dwarf_Rush_Normal:
                    return "P_Dwarf_Rush_Normal";
                case EnemyTypes.Enemy_Dwarf_Rush_Elite:
                    return "P_Dwarf_Rush_Elite";
                case EnemyTypes.Enemy_Dwarf_Rush_Guv:
                    return "P_Dwarf_Rush_Guv";

                case EnemyTypes.Enemy_Dwarf_Melee_Boss:
                    return "P_Dwarf_Melee_Boss";
                case EnemyTypes.Enemy_Dwarf_Rnaged_Boss:
                    return "P_Dwarf_Ranged_Boss";
                case EnemyTypes.Enemy_Dwarf_Rush_Boss:
                    return "P_Dwarf_Rush_Boss";
                //-- Act2 --

                default:
                    return null;
                #endregion
            }
        }
    }
    public string EnemeyGade
    {
        get
        {
            switch (enemyType)
            {
                #region 생략
                case EnemyTypes.Enemy_Koblod_Melee_Normal:
                case EnemyTypes.Enemy_Kobold_Ranged_Normal:
                case EnemyTypes.Enemy_Kobold_Rush_Normal:
                case EnemyTypes.Enemy_Dwarf_Melee_Normal:
                case EnemyTypes.Enemy_Dwarf_Ranged_Normal:
                case EnemyTypes.Enemy_Dwarf_Rush_Normal:
                    return "Normal";

                case EnemyTypes.Enemy_Kobold_Melee_Elite:
                case EnemyTypes.Enemy_Kobold_Ranged_Elite:
                case EnemyTypes.Enemy_Kobold_Rush_Elite:
                case EnemyTypes.Enemy_Dwarf_Melee_Elite:
                case EnemyTypes.Enemy_Dwarf_Ranged_Elite:
                case EnemyTypes.Enemy_Dwarf_Rush_Elite:
                    return "Elite";

                case EnemyTypes.Enemy_Kobold_Melee_Guv:
                case EnemyTypes.Enemy_Kobold_Ranged_Guv:
                case EnemyTypes.Enemy_Kobold_Rush_Guv:
                case EnemyTypes.Enemy_Dwarf_Melee_Guv:
                case EnemyTypes.Enemy_Dwarf_Ranged_Guv:
                case EnemyTypes.Enemy_Dwarf_Rush_Guv:
                    return "Guv";

                case EnemyTypes.Enemy_Kobold_Melee_Boss:
                case EnemyTypes.Enemy_Kobold_Rnaged_Boss:
                case EnemyTypes.Enemy_Kobold_Rush_Boss:
                case EnemyTypes.Enemy_Dwarf_Melee_Boss:
                case EnemyTypes.Enemy_Dwarf_Rnaged_Boss:
                case EnemyTypes.Enemy_Dwarf_Rush_Boss:
                    return "Boss";

                default:
                    return null;
                    #endregion
            }
        }
    }

    private EnemyTypeInfo Info;
    public EnemyTypeInfo SetInfo { set { Info = value; } }
    public string StageNum { get { return Info.StageNum; } }
    public string Name { get { return Info.Name; } }
    public GameObject Prefab { get { return Info.Prefab; } }
    public string Sound { get { return Info.Sound; } }
    public EnemyTypeInfo.AttackTypes AttackType { get { return Info.AttackType; } }
    public float AttackRange { get { return Info.AttackRange; } }
    public float AttackDelay { get { return Info.AttackDelay; } }
    public int EXP { get { return Info.EXP; } }
    //-- Stat --
    public int Level { get { return Info.Level; } }
    public int Attack { get { return Info.Attack; } }
    public int MaxHealth { get { return Info.MaxHealth; } }
    public int MoveSpeed { get { return Info.MoveSpeed; } }
    public int HitPercent { get { return Info.HitPercent; } }

    private void Awake()
    {
        Info = DataTableManager.Instance.GetEnemyType(strEnemyType);
    }
}
