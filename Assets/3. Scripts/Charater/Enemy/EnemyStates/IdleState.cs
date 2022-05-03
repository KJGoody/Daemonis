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

            // 일정 범위 안에 존재하는 오브젝트를 찾는다. 첫번째 값: 위치, 두번쨰 값: 작용범위
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
