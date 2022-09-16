using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public enum EnemyTypes
    {
        BaseMelee,
        BaseRanged,
        BaseRush,
        BaseAOE,
        Koblod_Melee,
        Koblod_Ranged
    }
    public EnemyTypes enemyType;

    public enum EnemyGrade { Normal, Elite, Guv }
    public EnemyGrade enemyGrade;

    [HideInInspector] public string Name;
    [HideInInspector] public string Sound;
    [HideInInspector] public float AttackRnage; // 공격 사거리
    [HideInInspector] public float AttackDelay; // 공격 딜레이
    [HideInInspector] public int EXP;

    private void Start()
    {

    }
}
