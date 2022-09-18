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
        #region ����
        BaseMelee,
        BaseRanged,
        BaseAOE,
        Kobold_Ranged
        #endregion
    }
    public AttackTypes AttackType;
    public float AttackRange; // ���� ��Ÿ�
    public float AttackDelay; // ���� ������
    public int EXP;
}
