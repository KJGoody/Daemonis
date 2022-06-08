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

    [HideInInspector]   // ��׷� ����
    public float AggroRnage;
    [HideInInspector]   // ���� �߻� ��ġ
    public Vector3 ExitPoint;
    [HideInInspector]   // ���� ��Ÿ�
    public float AttackRnage;
    [HideInInspector]   // ���� ������
    public float AttackDelay;


    void Awake()
    {
        switch (enemyType) 
        {
            case EnemyTypes.BaseMelee:
                AggroRnage = 2;
                ExitPoint = new Vector3(-0.1f, 0.2f, 0);
                AttackRnage = 1;
                AttackDelay = 1;

                break;

            case EnemyTypes.BaseRanged:
                AggroRnage = 3;
                ExitPoint = new Vector3(-0.1f, 0.2f, 0);
                AttackRnage = 3;
                AttackDelay = 2;
                break;

            case EnemyTypes.BaseRush:
                AggroRnage = 2;
                ExitPoint = new Vector3(-0.1f, 0.2f, 0);
                AttackRnage = 2;
                AttackDelay = 3;
                break;

            case EnemyTypes.BaseAOE:
                AggroRnage = 3;
                ExitPoint = new Vector3(-0.1f, 0.2f, 0);
                AttackRnage = 3;
                AttackDelay = 3;
                break;

            case EnemyTypes.Koblod_Melee:
                AggroRnage = 3;
                ExitPoint = new Vector3(-0.1f, 0.2f, 0);
                AttackRnage = 0.5f;
                AttackDelay = 1;
                break;

            case EnemyTypes.Koblod_Ranged:
                AggroRnage = 3;
                ExitPoint = new Vector3(-0.1f, 0.2f, 0);
                AttackRnage = 3;
                AttackDelay = 2;
                break;
        }
    }
}
