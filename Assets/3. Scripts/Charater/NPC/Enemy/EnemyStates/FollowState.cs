using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FollowState : IState
{
    private EnemyBase parent;

    public void Enter(EnemyBase parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

        // 플레이어에게 이동
        parent.Direction = parent.MyTarget.transform.position - parent.transform.position;

        if (distance <= parent.myAttackRange)  // 타겟과 자신의 거리가 공격 사거리 안에 있을 때 공격
        {
            parent.Direction = Vector2.zero;
            parent.ChangeState(new AttackState());
        }
    }
}
