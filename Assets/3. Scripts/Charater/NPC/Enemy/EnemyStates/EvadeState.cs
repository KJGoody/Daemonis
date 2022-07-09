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
        aNav = parent.GetComponent<ANav>();
        parent.StartCoroutine(aNav.StartPathFinding(parent.myStartPosition));
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.MyTarget = null;

        parent.StartCoroutine(aNav.WaitForPathFindingEnd());
    }

    public void Update()
    {
        if (aNav.EndPathFinding)
        {
            if (aNav.SucessPathFinding) // A*알고리즘이 경로 찾기를 성공 시
            {
                // 경로를 차례대로 받아옴
                parent.Direction = aNav.path[aNav.CurrentPathNode].worldPos - parent.transform.position;

                float distacne = Vector2.Distance(aNav.path[aNav.CurrentPathNode].worldPos, parent.transform.position);
                if(distacne < 0.5f) // 목표 지점 도착 시
                {
                    aNav.CurrentPathNode -= 1;  // 다음 목표 지점을 찾음
                    if (aNav.CurrentPathNode < 0) // 다음 목표가 없을 시 실행
                    {
                        parent.ChangeState(new IdleState());
                        aNav.ResetANav();
                    }
                }
            }
            else
            {
                parent.Direction = parent.myStartPosition - parent.transform.position;

                if(Vector2.Distance(parent.myStartPosition, parent.transform.position) < 0.5f)
                {
                    parent.ChangeState(new IdleState());
                    aNav.ResetANav();
                }
            }
        }
    }
}

