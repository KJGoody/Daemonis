using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{
    private EnemyBase parent;

    private ANav aNav;
    

    public void Enter(EnemyBase parent)
    {
        this.parent = parent;

        parent.CreateResource(Resources.Load("ANav") as GameObject, parent.transform);
        aNav = parent.GetComponentInChildren<ANav>();
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.Reset();
    }

    public void Update()
    {
        if (aNav.EndPathFinding)
        {
            parent.Direction = aNav.path[aNav.CurrentPathNode].worldPos - parent.transform.position;

            float distacne = Vector2.Distance(aNav.path[aNav.CurrentPathNode].worldPos, parent.transform.position);
            if(distacne < 0.1f)
            {
                aNav.CurrentPathNode -= 1;
                if (aNav.CurrentPathNode < 0)
                {
                    parent.ChangeState(new IdleState());
                    aNav.DestroyANav();
                }
            }
        }
    }
}

