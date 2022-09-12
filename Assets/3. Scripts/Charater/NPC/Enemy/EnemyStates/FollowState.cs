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

        //// Ÿ���� ��׷ι������� ��� �� OR �������������� �Ÿ��� ��׷� ���� +��ŭ �־����� ��� OR ���� ���� �ִ� �ð��� 2f �̻��� ��� �ǵ��ư�
        //if (distance > parent.myAggroRange + 3 || Vector2.Distance(parent.myStartPosition, parent.transform.position) > parent.myAggroRange + 5f || parent.RubbingTime > 2f)    
        //{
        //    parent.ChangeState(new EvadeState());
        //}

        if (parent.MyTarget != null)
        {
            parent.Direction = parent.MyTarget.transform.position - parent.transform.position;

            if (distance <= parent.myAttackRange)  // Ÿ�ٰ� �ڽ��� �Ÿ��� ���� ��Ÿ� �ȿ� ���� �� ����
            {
                parent.Direction = Vector2.zero;
                parent.ChangeState(new AttackState());
            }
        }
    }
}
