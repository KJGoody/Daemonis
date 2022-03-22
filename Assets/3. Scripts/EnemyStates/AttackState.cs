using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;

    private float attackCooldown = 1; // 공격 딜레이
    private float extraRange = 0.1f; // 공격 여유 거리
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;
            parent.StartCoroutine(Attack());
        }

        if (parent.MyTarget != null)
        {
            //Debug.Log("AttackState");

            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

            // 공격거리 보다 멀리있으면 Follow 상태로 변경한다.
            if (distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
        }

        else
        {
            // 공격 중에 타겟이 없으면 Idle상태로 변경한다.
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;
        parent._prefabs.PlayAnimation(4);
        //parent.Direction = Vector2.zero;

        yield return new WaitForSeconds(1); // 공격 후딜 넣으면 될듯
        
        parent.IsAttacking = false;
    }

}
