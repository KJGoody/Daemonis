using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    private float randomNumX;
    private float randomNumY;

    private float RubbingTime;

    public void Enter(Enemy parent)
    {
        this.parent = parent;

        randomNumX = Random.Range(-parent.MyAttackRange / 2, parent.MyAttackRange / 2);
        randomNumY = Random.Range(-parent.MyAttackRange / 2, parent.MyAttackRange / 2);

    }

    public void Exit()
    {

    }

    public void Update()
    {
        RaycastHit2D Hit = Physics2D.Raycast(parent.transform.position + new Vector3(0, 0.2f, 0), new Vector3((parent.MyTarget.transform.position.x - parent.transform.position.x), (parent.MyTarget.transform.position.y - parent.transform.position.y), 0).normalized, 0.4f, LayerMask.GetMask("Wall"));
        if (Hit.collider != null)
        {
            RubbingTime += Time.deltaTime;
            if (RubbingTime > 3f)
                parent.ChangeState(new EvadeState());
        }
        else
            RubbingTime = 0f;

        if (parent.MyTarget != null)
        {
            parent.Direction = new Vector3(parent.MyTarget.position.x + randomNumX, parent.MyTarget.position.y + randomNumY, parent.MyTarget.position.z) - parent.transform.position;            // 사거리 안에 들면 공격상태로 변경
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);
            if (distance <= parent.MyAttackRange)
            {
                parent.Direction = Vector2.zero;
                parent.ChangeState(new AttackState());
            }
        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        }
    }
}
