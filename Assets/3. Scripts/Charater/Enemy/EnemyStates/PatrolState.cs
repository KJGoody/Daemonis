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
        aNav = parent.GetComponent<ANav>();
        PatrolPointPathFinding();
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        // ��� Ž�� ���� �� ����
        if (aNav.EndPathFinding)
        {
            // ���� Ž�� ���� �� ����
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
                        aNav.ResetANav();
                    }
                }
            }
            // ��� Ž�� ���� �� �������� �� Ž��
            else
                PatrolPointPathFinding();
        }

        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
            parent.StartCoroutine(aNav.WaitForPathFindingEnd());
        }
    }

    private void PatrolPointPathFinding()
    {
        parent.StartCoroutine(aNav.WaitForPathFindingEnd());

        while (true)
        {
            // ��׷� ���� �� ���� ������ ����
            PatrolPoint = Random.insideUnitCircle * parent.myAggroRange;
            PatrolPoint += parent.myStartPosition;

            Collider2D collider = Physics2D.OverlapCircle(PatrolPoint, 0.5f, LayerMask.GetMask("Wall"));
            // �ش������� ���� �ƴ� �� AND ���������� �ʹ� ������ ���� ���(�ʹ� ����� ��� ���� �߻�)
            if (collider == null && Vector2.Distance(PatrolPoint, parent.transform.position) > 1f) break;
        }

        parent.StartCoroutine(aNav.StartPathFinding(PatrolPoint));
    }
}
