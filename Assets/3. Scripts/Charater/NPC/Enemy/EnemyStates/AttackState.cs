using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private EnemyBase parent;
    private float attackCooldown; // 공격 딜레이

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
        // 공격거리 보다 멀리있으면 Follow 상태로 변경한다.
        if (distance >= parent.myAttackRange * 1.1f && !parent.IsAttacking) // 플레이어 사거리 + 공격 여유 거리(플레이어 사거리 * 0.1f) = 플레이어 인식 벗어나는 거리
        {
            parent.ChangeState(new FollowState());
        }

        // 공격시 플레이어 시선처리
        parent.LookAtTarget();

        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;
            parent.IsAttacking = true;
            switch (parent.enemytype.AttackType)                       // 애니미타입에 따라 공격 모션을 다르게 설정
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
        yield return new WaitForSeconds(0.5f); // 선딜
        parent._prefabs.PlayAnimation(4);

        yield return new WaitForSeconds(0.15f); // 애니메이션 내려찍기 시작
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_BaseMelee") as GameObject, parent.ExitPoint);
        yield return new WaitForSeconds(0.15f); // 애니메이션 종료

        parent.IsAttacking = false;
    }

    private IEnumerator rangedAttack()
    {
        yield return new WaitForSeconds(0.5f); // 선딜
        parent._prefabs.PlayAnimation(5);

        yield return new WaitForSeconds(0.2f);
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_BaseRanged") as GameObject, parent.ExitPoint);
        yield return new WaitForSeconds(0.1f);

        parent.IsAttacking = false;
    }

    private IEnumerator RushAttack()
    {
        parent._prefabs.PlayAnimation(4);           // 선딜 모션
        yield return new WaitForSeconds(1f);        // 선딜

        parent.IsRushing = true;
        parent.gameObject.layer = 7;                // Rushing레이어로 바꾸기, 플레이어와 충돌무시
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
        yield return new WaitForSeconds(0.5f); // 선딜
        parent._prefabs.PlayAnimation(6);
        yield return new WaitForSeconds(1f);
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_BaseAOE") as GameObject, parent.MyTarget);
        parent.IsAttacking = false;
    }

    private IEnumerator Kobold_Ranged()
    {
        yield return new WaitForSeconds(0.5f); // 선딜
        parent._prefabs.PlayAnimation(4);

        yield return new WaitForSeconds(0.15f);
        parent.InstantiateAttack(Resources.Load("Prefabs/EnemyAttack/P_A_Kobold_Ranged") as GameObject, parent.ExitPoint);      
        yield return new WaitForSeconds(0.15f);

        parent.IsAttacking = false;
    }
}
