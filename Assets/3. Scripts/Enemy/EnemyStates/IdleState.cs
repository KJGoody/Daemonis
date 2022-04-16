using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IdleState : IState
{
    private Enemy parent;
    private Coroutine PlayCoroutine;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.parent.Reset();
        PlayCoroutine = parent.StartCoroutine(NextPatrol());
    }

    public void Exit()
    {
        parent.StopCoroutine(PlayCoroutine);

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
