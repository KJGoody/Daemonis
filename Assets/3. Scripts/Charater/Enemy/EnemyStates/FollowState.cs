using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FollowState : IState
{
    private EnemyBase parent;

    private float RubbingTime;

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

        if (parent.MyTarget != null)
        {
            parent.Direction = parent.MyTarget.transform.position - parent.transform.position;            // 사거리 안에 들면 공격상태로 변경

            if (distance <= parent.myAttackRange)  // 타겟과 자신의 거리가 공격 사거리 안에 있을 때 공격
            {
                parent.Direction = Vector2.zero;
                parent.ChangeState(new AttackState());
            }
        }

        if (distance > parent.myAggroRange)    // 타겟이 어그로범위에서 벗어날 시 되돌아감
        {
            parent.ChangeState(new EvadeState());
        }



        RaycastHit2D Hit = Physics2D.Raycast(parent.transform.position + new Vector3(0, 0.2f, 0), new Vector3((parent.MyTarget.transform.position.x - parent.transform.position.x), (parent.MyTarget.transform.position.y - parent.transform.position.y), 0).normalized, 0.4f, LayerMask.GetMask("Wall"));
        if (Hit.collider != null)
        {
            RubbingTime += Time.deltaTime;
            if (RubbingTime > 3f)
                parent.ChangeState(new EvadeState());
        }
        else
            RubbingTime = 0f;

    }
}
