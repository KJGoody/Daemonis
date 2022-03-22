using UnityEngine;

class FollowState : IState
{
    private Enemy parent;


    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        //parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        Debug.Log("FollowState");
        if (parent.MyTarget != null)
        {

            parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;
            //Vector2 targetPosition = parent.Target.position;
            //Vector2 myPosition = parent.transform.position;
            //parent.Direction = targetPosition - myPosition;
            // 사거리 안에 들면 공격상태로 변경
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
