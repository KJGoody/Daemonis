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

        // �÷��̾�� �̵�
        parent.Direction = parent.MyTarget.transform.position - parent.transform.position;

        if (distance <= parent.myAttackRange)  // Ÿ�ٰ� �ڽ��� �Ÿ��� ���� ��Ÿ� �ȿ� ���� �� ����
        {
            parent.Direction = Vector2.zero;
            parent.ChangeState(new AttackState());
        }
    }
}
