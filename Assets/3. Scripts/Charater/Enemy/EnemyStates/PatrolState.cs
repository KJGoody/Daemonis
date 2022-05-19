using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PatrolState : IState
{
    private EnemyBase parent;
    private ANav aNav;

    private Vector3 PatrolPoint;

    public void Enter(EnemyBase parent)
    {
        this.parent = parent;

        PatrolPointPathFinding();
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if (aNav.EndPathFinding)
        {
            if (aNav.SucessPathFinding)
            {
                parent.Direction = aNav.path[aNav.CurrentPathNode].worldPos - parent.transform.position;

                float distacne = Vector2.Distance(aNav.path[aNav.CurrentPathNode].worldPos, parent.transform.position);
                if (distacne < 0.5f)
                {
                    aNav.CurrentPathNode -= 1;
                    if (aNav.CurrentPathNode < 0)
                    {
                        parent.ChangeState(new IdleState());
                        aNav.DestroyANav();
                    }
                }
            }
            else
            {
                PatrolPointPathFinding();
            }
        }

        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
            aNav.DestroyANav();
        }
    }

    private void PatrolPointPathFinding()
    {
        if (aNav != null) aNav.DestroyANav();

        while (true)
        {
            PatrolPoint = Random.insideUnitCircle * parent.myAggroRange;
            //PatrolPoint = Random.insideUnitCircle;
            PatrolPoint += parent.myStartPosition;

            Collider2D collider = Physics2D.OverlapCircle(PatrolPoint, 0.5f, LayerMask.GetMask("Wall"));
            if (collider == null && Vector2.Distance(PatrolPoint, parent.transform.position) > 1f) break;
        }

        parent.CreateResource(Resources.Load("ANav") as GameObject, parent.transform);
        aNav = parent.GetComponentInChildren<ANav>();
        aNav.TargetPoint = PatrolPoint;
    }
}
