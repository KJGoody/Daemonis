using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FollowState : IState
{
    private EnemyBase parent;

    public void Enter(EnemyBase parent)
    {
        this.parent = parent;
        parent.RubbingTime = 0f;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

        //// 타겟이 어그로범위에서 벗어날 시 OR 시작지점까지의 거리가 어그로 범위 +만큼 멀어졌을 경우 OR 벽에 비비고 있는 시간이 2f 이상인 경우 되돌아감
        //if (distance > parent.myAggroRange + 3 || Vector2.Distance(parent.myStartPosition, parent.transform.position) > parent.myAggroRange + 5f || parent.RubbingTime > 2f)    
        //{
        //    parent.ChangeState(new EvadeState());
        //}

        if (parent.MyTarget != null)
        {
            parent.Direction = parent.MyTarget.transform.position - parent.transform.position;

            if (distance <= parent.myAttackRange)  // 타겟과 자신의 거리가 공격 사거리 안에 있을 때 공격
            {
                parent.Direction = Vector2.zero;
                parent.ChangeState(new AttackState());
            }
        }
    }
}
