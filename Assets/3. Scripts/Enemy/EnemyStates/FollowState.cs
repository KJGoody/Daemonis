using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    private Vector3 AggroPoint;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        float randomNumX = Random.Range(0, parent.MyAttackRange);
        float randomNumY = Random.Range(0, parent.MyAttackRange);
        AggroPoint = new Vector3(parent.MyTarget.position.x + randomNumX, parent.MyTarget.position.y + randomNumY, parent.MyTarget.position.z);
    }

    public void Exit()
    {
        //parent.Direction = Vector2.zero;

    }

    public void Update()
    {
        //Debug.Log("FollowState");
        if (parent.MyTarget != null)
        {
            while(true)
            {
                float radius = (AggroPoint - parent.MyTarget.transform.position).sqrMagnitude;
                if (radius < parent.MyAttackRange * parent.MyAttackRange)
                    break;
                float randomNumX = Random.Range(0, parent.MyAttackRange);
                float randomNumY = Random.Range(0, parent.MyAttackRange);
                AggroPoint = new Vector3(parent.MyTarget.position.x + randomNumX, parent.MyTarget.position.y + randomNumY, parent.MyTarget.position.z);
            }

            //Vector2 targetPosition = parent.Target.position;
            //Vector2 myPosition = parent.transform.position;
            //parent.Direction = targetPosition - myPosition;

            parent.Direction = AggroPoint - parent.transform.position;            // 사거리 안에 들면 공격상태로 변경

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
