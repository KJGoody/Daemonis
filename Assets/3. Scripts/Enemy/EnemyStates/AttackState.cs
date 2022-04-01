using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    private Transform source;

    private float attackCooldown; // 공격 딜레이
    private float extraRange = 0.1f; // 공격 여유 거리    // 공격 여유 거리 + 인식거리 = 플레이어 인식 벗어나는 거리

    public void Enter(Enemy parent)
    {
        this.parent = parent;

        switch (parent.enemyType)               // 애니미타입에 따라 공격 딜레이 다르게 주기
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
            switch (parent.enemyType)                       // 애니미타입에 따라 공격 모션을 다르게 설정
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

   
    public IEnumerator meleeAttack()
    {
        parent.IsAttacking = true;
        parent._prefabs.PlayAnimation(4);

        yield return new WaitForSeconds(0.15f); // 애니메이션 내려찍기 시작
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/MeleeAttack1") as GameObject, parent.exitPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.15f); // 애니메이션 종료
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
        parent.gameObject.layer = 7; // Rushing레이어로 바꾸기, 플레이어와 충돌무시
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/RushAttack1") as GameObject, parent.exitPoint.position, Quaternion.identity);
        parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;
        yield return new WaitForSeconds(0.5f); 

        parent.IsRushing = false;
        parent.gameObject.layer = 6;
        parent.IsAttacking = false;

    }
}
