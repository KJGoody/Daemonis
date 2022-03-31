using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    private Transform source;
    private int damage;
    private Vector2 direction;

    private float attackCooldown; // ���� ������
    private float extraRange = 0.1f; // ���� ���� �Ÿ�    // ���� ���� �Ÿ� + �νİŸ� = �÷��̾� �ν� ����� �Ÿ�

    public void Enter(Enemy parent)
    {
        this.parent = parent;

        switch (parent.enemyType)               // �ִϹ�Ÿ�Կ� ���� ���� ������ �ٸ��� �ֱ�
        {
            case Enemy.EnemyType.kobold_melee:
                    attackCooldown = 1;
                    break;

            case Enemy.EnemyType.kobold_ranged:
                    attackCooldown = 2;
                    break;
        }
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;
            switch (parent.enemyType)                       // �ִϹ�Ÿ�Կ� ���� ���� ����� �ٸ��� ����
            {
                case Enemy.EnemyType.kobold_melee:
                    parent.StartCoroutine(meleeAttack());
                    break;

                case Enemy.EnemyType.kobold_ranged:
                    parent.StartCoroutine(rangedAttack());
                    break;
            }
        }

        if (parent.MyTarget != null)
        {
            //Debug.Log("AttackState")
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

   
    public IEnumerator meleeAttack()
    {
        Transform currentTarget = parent.MyTarget;

        parent.IsAttacking = true;
        parent.FindTarget();
        parent._prefabs.PlayAnimation(4);

        parent.EnemyAttackResource(Resources.Load("EnemyAttack/MeleeAttack1") as GameObject, parent.exitPoint.position, Quaternion.identity);
        if(currentTarget)
        {
           
        }
        yield return new WaitForSeconds(0.3f); // �ִϸ��̼� ����
        parent.IsAttacking = false;
    }
    
    public IEnumerator rangedAttack()
    {
       parent.IsAttacking = true;
       parent._prefabs.PlayAnimation(5);

       yield return new WaitForSeconds(0.6f); 
        
       parent.IsAttacking = false;
    }

}
