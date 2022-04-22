using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private EnemyBase parent;
    private float attackCooldown; // ���� ������

    public void Enter(EnemyBase parent)
    {
        this.parent = parent;

        attackCooldown = parent.enemytype.AttackDelay;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if ((parent.MyTarget.transform.position - parent.transform.position).normalized.x > 0)
            parent._prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if ((parent.MyTarget.transform.position - parent.transform.position).normalized.x < 0)
            parent._prefabs.transform.localScale = new Vector3(1, 1, 1);
        // ���ݽ� �÷��̾� �ü�ó��

        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;
            parent.IsAttacking = true;
            switch (parent.enemytype.enemyType)                       // �ִϹ�Ÿ�Կ� ���� ���� ����� �ٸ��� ����
            {
                case EnemyType.EnemyTypes.BaseMelee:
                    parent.StartCoroutine(meleeAttack());
                    break;

                case EnemyType.EnemyTypes.BaseRanged:
                    parent.StartCoroutine(rangedAttack());
                    break;

                case EnemyType.EnemyTypes.BaseRush:
                    parent.StartCoroutine(RushAttack());
                    break;

                case EnemyType.EnemyTypes.BaseAOE:
                    parent.StartCoroutine(AOEAttack());
                    break;
            }
        }

        if (parent.MyTarget != null)
        {
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);
            // ���ݰŸ� ���� �ָ������� Follow ���·� �����Ѵ�.
            if (distance >= parent.myAttackRange * 1.1f && !parent.IsAttacking) // �÷��̾� ��Ÿ� + ���� ���� �Ÿ�(�÷��̾� ��Ÿ� * 0.1f) = �÷��̾� �ν� ����� �Ÿ�
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


    IEnumerator meleeAttack()
    {
        yield return new WaitForSeconds(0.5f); // ����
        parent._prefabs.PlayAnimation(4);

        yield return new WaitForSeconds(0.15f); // �ִϸ��̼� ������� ����
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/BaseMelee_Attack") as GameObject, parent.ExitPoint);
        yield return new WaitForSeconds(0.15f); // �ִϸ��̼� ����

        parent.IsAttacking = false;
    }

    IEnumerator rangedAttack()
    {
        yield return new WaitForSeconds(0.5f); // ����
        parent._prefabs.PlayAnimation(5);

        yield return new WaitForSeconds(0.2f);
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/BaseRanged_Attack") as GameObject, parent.ExitPoint);
        yield return new WaitForSeconds(0.1f);

        parent.IsAttacking = false;
    }

    IEnumerator RushAttack()
    {
        parent._prefabs.PlayAnimation(4);           // ���� ���
        yield return new WaitForSeconds(1f);        // ����

        parent.IsRushing = true;
        parent.gameObject.layer = 7;                // Rushing���̾�� �ٲٱ�, �÷��̾�� �浹����
        parent.Direction = parent.MyTarget.transform.position - parent.transform.position;
        parent.RushSpeed = 7f;
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/BaseRush_Attack") as GameObject, parent.ExitPoint);
        yield return new WaitForSeconds(0.5f);

        parent.Direction = Vector2.zero;
        parent.IsRushing = false;
        parent.gameObject.layer = 6;
        parent.IsAttacking = false;
    }

    IEnumerator AOEAttack()
    {
        yield return new WaitForSeconds(0.5f); // ����
        parent._prefabs.PlayAnimation(6);
        yield return new WaitForSeconds(1f);
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/BaseAOE_Attack") as GameObject, parent.MyTarget);
        parent.IsAttacking = false;
    }
}
