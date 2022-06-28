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
            if (aNav.SucessPathFinding) // A*�˰����� ��� ã�⸦ ���� ��
            {
                // ��θ� ���ʴ�� �޾ƿ�
                parent.Direction = aNav.path[aNav.CurrentPathNode].worldPos - parent.transform.position;

                float distacne = Vector2.Distance(aNav.path[aNav.CurrentPathNode].worldPos, parent.transform.position);
                if(distacne < 0.5f) // ��ǥ ���� ���� ��
                {
                    aNav.CurrentPathNode -= 1;  // ���� ��ǥ ������ ã��
                    if (aNav.CurrentPathNode < 0) // ���� ��ǥ�� ���� �� ����
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

