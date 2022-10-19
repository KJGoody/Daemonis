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
        float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);
        // ���ݰŸ� ���� �ָ������� Follow ���·� �����Ѵ�.
        if (distance >= parent.myAttackRange * 1.1f && !parent.IsAttacking) // �÷��̾� ��Ÿ� + ���� ���� �Ÿ�(�÷��̾� ��Ÿ� * 0.1f) = �÷��̾� �ν� ����� �Ÿ�
        {
            parent.ChangeState(new FollowState());
        }

        // ���ݽ� �÷��̾� �ü�ó��
        parent.LookAtTarget();

        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;
            parent.IsAttacking = true;
            switch (parent.enemytype.AttackType)                       // �ִϹ�Ÿ�Կ� ���� ���� ����� �ٸ��� ����
            {
                case EnemyTypeInfo.AttackTypes.BaseMelee:
                    parent.StartCoroutine(meleeAttack());
                    break;

                case EnemyTypeInfo.AttackTypes.BaseRanged:
                    parent.StartCoroutine(rangedAttack());
                    break;

                case EnemyTypeInfo.AttackTypes.BaseAOE:
                    parent.StartCoroutine(AOEAttack());
                    break;

                case EnemyTypeInfo.AttackTypes.Kobold_Ranged:
                    parent.StartCoroutine(Kobold_Ranged());
                    break;

                case EnemyTypeInfo.AttackTypes.BaseRush:
                    parent.StartCoroutine(RushAttack());
                    break;
            }
        }
    }

    private IEnumerator meleeAttack()
    {
        yield return new WaitForSeconds(0.5f); // ����
        parent._prefabs.PlayAnimation(4);

        yield return new WaitForSeconds(0.15f); // �ִϸ��̼� ������� ����
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_BaseMelee") as GameObject, parent.ExitPoint);
        yield return new WaitForSeconds(0.15f); // �ִϸ��̼� ����

        parent.IsAttacking = false;
    }

    private IEnumerator rangedAttack()
    {
        yield return new WaitForSeconds(0.5f); // ����
        parent._prefabs.PlayAnimation(5);

        yield return new WaitForSeconds(0.2f);
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_BaseRanged") as GameObject, parent.ExitPoint);
        yield return new WaitForSeconds(0.1f);

        parent.IsAttacking = false;
    }

    private IEnumerator RushAttack()
    {
        parent._prefabs.PlayAnimation(4);           // ���� ���
        yield return new WaitForSeconds(1f);        // ����

        parent.IsRushing = true;
        parent.gameObject.layer = 7;                // Rushing���̾�� �ٲٱ�, �÷��̾�� �浹����
        parent.Direction = parent.MyTarget.transform.position - parent.transform.position;
        parent.RushSpeed = 7f;
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_BaseRush") as GameObject, parent.transform);
        yield return new WaitForSeconds(0.5f);

        parent.Direction = Vector2.zero;
        parent.IsRushing = false;
        parent.gameObject.layer = 6;
        parent.IsAttacking = false;
    }

    private IEnumerator AOEAttack()
    {
        yield return new WaitForSeconds(0.5f); // ����
        parent._prefabs.PlayAnimation(6);
        yield return new WaitForSeconds(1f);
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_BaseAOE") as GameObject, parent.MyTarget);
        parent.IsAttacking = false;
    }

    private IEnumerator Kobold_Ranged()
    {
        yield return new WaitForSeconds(0.5f); // ����
        parent._prefabs.PlayAnimation(4);

        yield return new WaitForSeconds(0.15f);
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_Kobold_Ranged") as GameObject, parent.ExitPoint);      
        yield return new WaitForSeconds(0.15f);

        parent.IsAttacking = false;
    }
}
