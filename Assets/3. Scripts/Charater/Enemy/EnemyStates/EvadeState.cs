using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{

    private EnemyBase parent;



    public void Enter(EnemyBase parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.Reset();
    }

    public void Update()
    {
        // 매프레임마다 처음 시작위치로 되돌아감.
        parent.Direction = parent.myStartPosition - parent.transform.position;

        // 시작 위치까지 이동하면 IdleState 상태로 변경시킴
        float distance = Vector2.Distance(parent.myStartPosition, parent.transform.position);
        if (distance <= 0.5f)
        {
            parent.ChangeState(new IdleState());
        }
    }
}

