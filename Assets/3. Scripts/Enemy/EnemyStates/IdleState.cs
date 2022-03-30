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
        //Debug.Log("IdleState");// ��������� ������ ���� �ӽ��ڵ�
        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    }
}
