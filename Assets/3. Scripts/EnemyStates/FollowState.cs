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
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if (parent.Target != null)
        {
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;

            Vector2 targetPosition = parent.Target.position;
            Vector2 myPosition = parent.transform.position;
            parent.Direction = targetPosition - myPosition;
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }
}
