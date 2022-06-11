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
        // 경로 탐색 종료 시 실행
        if (aNav.EndPathFinding)
        {
            // 경토 탐색 성공 시 실행
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
            // 경로 탐색 실패 시 정찰지점 재 탐색
            else
                PatrolPointPathFinding();
        }

        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
            aNav.DestroyANav();
        }
    }

    private void PatrolPointPathFinding()
    {   
        if (aNav != null) aNav.DestroyANav();   // 이미 A*알고리즘이 실행중이라면 해당알고리즘을 폐기

        while (true)
        {
            // 어그로 범위 안 랜덤 지점을 생성
            PatrolPoint = Random.insideUnitCircle * parent.myAggroRange;
            PatrolPoint += parent.myStartPosition;

            Collider2D collider = Physics2D.OverlapCircle(PatrolPoint, 0.5f, LayerMask.GetMask("Wall"));
            // 해당지점이 벽이 아닌 곳 AND 정찰지점이 너무 가깝지 않을 경우(너무 가까울 경우 오류 발생)
            if (collider == null && Vector2.Distance(PatrolPoint, parent.transform.position) > 1f) break;
        }

        parent.CreateResource(Resources.Load("ANav") as GameObject, parent.transform);
        aNav = parent.GetComponentInChildren<ANav>();
        aNav.TargetPoint = PatrolPoint;
    }
}
