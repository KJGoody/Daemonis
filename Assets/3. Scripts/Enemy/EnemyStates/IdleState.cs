using UnityEngine;
class IdleState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.parent.Reset();
    }

    public void Exit()
    {

    }

    public void Update()
    {
        //Debug.Log("IdleState");// 어떤상태인지 보려고 만든 임시코드
        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    }
}
