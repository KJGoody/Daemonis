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
        // A*알고리즘을 사용하기위한 오브젝트 생성후 스크립트 받아오기
        parent.CreateResource(Resources.Load("ANav") as GameObject, parent.transform);
        aNav = parent.GetComponentInChildren<ANav>();   // 움직일 대상 전달
        aNav.TargetPoint = parent.myStartPosition;      // 목표지점 설정
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.MyTarget = null;
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
                        aNav.DestroyANav();
                    }
                }
            }
            else
            {
                parent.Direction = parent.myStartPosition - parent.transform.position;

                if(Vector2.Distance(parent.myStartPosition, parent.transform.position) < 0.5f)
                {
                    parent.ChangeState(new IdleState());
                    aNav.DestroyANav();
                }
            }
        }
    }
}

