using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IdleState : IState
{
    private EnemyBase parent;
    private Coroutine courrentCoroutine;

    public void Enter(EnemyBase parent)
    {
        this.parent = parent;

            // ���� ���� �ȿ� �����ϴ� ������Ʈ�� ã�´�. ù��° ��: ��ġ, �ι��� ��: �ۿ����
        Collider2D collider = Physics2D.OverlapCircle(parent.transform.position, parent.myAggroRange, LayerMask.GetMask("Player"));
        if (collider != null)
            parent.SetTarget(collider.transform);

        //courrentCoroutine = parent.StartCoroutine(NextPatrol());
    }

    public void Exit()
    {
        //parent.StopCoroutine(courrentCoroutine);
    }

    public void Update()
    {
        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    }

    IEnumerator NextPatrol()
    {
        float NextPatrolSeconds = Random.Range(3f, 6f);
        yield return new WaitForSeconds(NextPatrolSeconds);
        parent.ChangeState(new PatrolState());
    }
}
