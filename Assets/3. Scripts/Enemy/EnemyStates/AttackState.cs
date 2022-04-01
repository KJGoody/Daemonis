using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    private Transform source;

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

            case Enemy.EnemyType.Kobold_rush:
                attackCooldown = 3;
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

                case Enemy.EnemyType.Kobold_rush:
                    parent.StartCoroutine(RushAttack());
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
        parent.IsAttacking = true;
        parent._prefabs.PlayAnimation(4);

        yield return new WaitForSeconds(0.15f); // �ִϸ��̼� ������� ����
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/MeleeAttack1") as GameObject, parent.exitPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.15f); // �ִϸ��̼� ����
        parent.IsAttacking = false;
    }
    
    public IEnumerator rangedAttack()
    {
        parent.IsAttacking = true;
        parent._prefabs.PlayAnimation(5);

        yield return new WaitForSeconds(0.2f); 
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/RangedAttack1") as GameObject, parent.exitPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f); 
        
       parent.IsAttacking = false;
    }

    public IEnumerator RushAttack()
    {
        parent.IsAttacking = true;
        parent._prefabs.PlayAnimation(4);
        yield return new WaitForSeconds(1f);

        parent.IsRushing = true;
        parent.gameObject.layer = 7; // Rushing���̾�� �ٲٱ�, �÷��̾�� �浹����
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/RushAttack1") as GameObject, parent.exitPoint.position, Quaternion.identity);
        parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;
        yield return new WaitForSeconds(0.5f); 

        parent.IsRushing = false;
        parent.gameObject.layer = 6;
        parent.IsAttacking = false;

    }
}
