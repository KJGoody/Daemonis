using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    private float randomNumX;
    private float randomNumY;

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
