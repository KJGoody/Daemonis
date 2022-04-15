using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{

    private Enemy parent;



    public void Enter(Enemy parent)
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
        parent.Direction = parent.MyStartPosition - parent.transform.position;

        // 매프레임마다 처음 시작위치로 되돌아감.
        Vector2 parentPosition = parent.transform.position;
        Vector2 startPosition = parent.MyStartPosition;
        //parent.transform.position
        //    = Vector2.MoveTowards(parentPosition, startPosition, parent.Speed * Time.deltaTime);

        // 시작 위치까지 이동하면 IdleState 상태로 변경시킴
        float distance = Vector2.Distance(startPosition, parentPosition);
        if (distance <= 0.5f)
        {
            parent.ChangeState(new IdleState());
        }

    }
}

