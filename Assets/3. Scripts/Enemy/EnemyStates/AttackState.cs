using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    public Transform MyTargert { get; set; }
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
        Transform currentTarget = MyTargert;

        parent.IsAttacking = true;
        parent._prefabs.PlayAnimation(4);
        //parent.Direction = Vector2.zero;
        yield return new WaitForSeconds(0.15f); // �ִϸ��̼� ������� ����
        parent.EnemyAttackBox.GetComponent<BoxCollider2D>().enabled = true; // EnemyAttackBox �ݶ��̴� Ȱ��ȭ
        yield return new WaitForSeconds(0.15f); // �ִϸ��̼� ����
        parent.EnemyAttackBox.GetComponent<BoxCollider2D>().enabled = false; // EnemyAttackBox �ݶ��̴� ��Ȱ��ȭ



        parent.IsAttacking = false;
    }
    
    public IEnumerator rangedAttack()
    {
        parent.IsAttacking = true;
        parent._prefabs.PlayAnimation(5);
       parent.Direction = Vector2.zero;

        yield return new WaitForSeconds(0.6f); 
        
        parent.IsAttacking = false;
    }

}
