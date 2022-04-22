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
        // �������Ӹ��� ó�� ������ġ�� �ǵ��ư�.
        parent.Direction = parent.myStartPosition - parent.transform.position;

        Vector2 parentPosition = parent.transform.position;
        Vector2 startPosition = parent.myStartPosition;

        // ���� ��ġ���� �̵��ϸ� IdleState ���·� �����Ŵ
        float distance = Vector2.Distance(startPosition, parentPosition);
        if (distance <= 0.5f)
        {
            parent.ChangeState(new IdleState());
        }

    }
}

