using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;

    private float attackCooldown = 1; // ���� ������
    private float extraRange = 0.1f; // ���� ���� �Ÿ�    // ���� ���� �Ÿ� + �νİŸ� = �÷��̾� �ν� ����� �Ÿ�
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

            // ���ݰŸ� ���� �ָ������� Follow ���·� �����Ѵ�.
            if (distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
        }

        else
        {
            // ���� �߿� Ÿ���� ������ Idle���·� �����Ѵ�.
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;
        parent._prefabs.PlayAnimation(4);
        //parent.Direction = Vector2.zero;

        yield return new WaitForSeconds(1); // ���� �ĵ� ������ �ɵ�
        
        parent.IsAttacking = false;
    }

}