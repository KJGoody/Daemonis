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
        // �������Ӹ��� ó�� ������ġ�� �ǵ��ư�.
        parent.Direction = parent.MyStartPosition - parent.transform.position;

        Vector2 parentPosition = parent.transform.position;
        Vector2 startPosition = parent.MyStartPosition;
        //parent.transform.position
        //    = Vector2.MoveTowards(parentPosition, startPosition, parent.Speed * Time.deltaTime);

        // ���� ��ġ���� �̵��ϸ� IdleState ���·� �����Ŵ
        float distance = Vector2.Distance(startPosition, parentPosition);
        if (distance <= 0.5f)
        {
            parent.ChangeState(new IdleState());
        }

    }
}

