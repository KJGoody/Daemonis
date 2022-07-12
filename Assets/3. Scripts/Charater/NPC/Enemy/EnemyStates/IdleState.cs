using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IdleState : IState
{
    private EnemyBase parent;
    private Coroutine currentCoroutine;

    public void Enter(EnemyBase parent)
    {
        this.parent = parent;

        // ���� ���� �ȿ� �����ϴ� ������Ʈ�� ã�´�. ù��° ��: ��ġ, �ι��� ��: �ۿ����
        Collider2D collider = Physics2D.OverlapCircle(parent.transform.position, parent.myAggroRange / 2, LayerMask.GetMask("PlayerHitBox"));
        if (collider != null && collider.CompareTag("Player"))
            parent.SetTarget(collider.transform);
        if(parent.gameObject.activeSelf)
            currentCoroutine = parent.StartCoroutine(NextPatrol());
    }

    public void Exit()
    {
        if(currentCoroutine != null)
            parent.StopCoroutine(currentCoroutine);
    }

    public void Update()
    {
        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    }

    IEnumerator NextPatrol()
    {   // �����ð� �� ���� ���·� ����
        float NextPatrolSeconds = Random.Range(5f, 15f);
        yield return new WaitForSeconds(NextPatrolSeconds);
        parent.ChangeState(new PatrolState());
    }
}