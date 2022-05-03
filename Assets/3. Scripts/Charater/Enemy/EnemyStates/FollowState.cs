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
            parent.Direction = parent.MyTarget.transform.position - parent.transform.position;            // ��Ÿ� �ȿ� ��� ���ݻ��·� ����

            if (distance <= parent.myAttackRange)  // Ÿ�ٰ� �ڽ��� �Ÿ��� ���� ��Ÿ� �ȿ� ���� �� ����
            {
                parent.Direction = Vector2.zero;
                parent.ChangeState(new AttackState());
            }
        }

        if (distance > parent.myAggroRange)    // Ÿ���� ��׷ι������� ��� �� �ǵ��ư�
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
