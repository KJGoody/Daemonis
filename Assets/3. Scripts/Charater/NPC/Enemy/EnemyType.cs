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
    [HideInInspector] public float AttackRnage; // ���� ��Ÿ�
    [HideInInspector] public float AttackDelay; // ���� ������
    [HideInInspector] public int EXP;

    private void Start()
    {

    }
}
