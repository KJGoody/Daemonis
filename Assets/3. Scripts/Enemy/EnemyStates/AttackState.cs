using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    private float attackCooldown; // 공격 딜레이
    private float extraRange = 0.1f; // 공격 여유 거리    // 공격 여유 거리 + 인식거리 = 플레이어 인식 벗어나는 거리

    public void Enter(Enemy parent)
    {
        this.parent = parent;

        switch (parent.enemyType)               // 애니미타입에 따라 공격 딜레이 다르게 주기
        {
            case Enemy.EnemyType.Basemelee:
                attackCooldown = 1;
                break;

            case Enemy.EnemyType.Baseranged:
                attackCooldown = 2;
                break;

            case Enemy.EnemyType.Baserush:
                attackCooldown = 3;
                break;

            case Enemy.EnemyType.BaseAOE:
                attackCooldown = 5;
                break;
        }
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if ((parent.MyTarget.transform.position - parent.transform.position).normalized.x > 0)
            parent._prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if((parent.MyTarget.transform.position - parent.transform.position).normalized.x < 0)
            parent._prefabs.transform.localScale = new Vector3(1, 1, 1);
        // 공격시 플레이어 시선처리
        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;
            parent.IsAttacking = true;
            switch (parent.enemyType)                       // 애니미타입에 따라 공격 모션을 다르게 설정
            {
                case Enemy.EnemyType.Basemelee:
                    parent.StartCoroutine(meleeAttack());
                    break;

                case Enemy.EnemyType.Baseranged:
                    parent.StartCoroutine(rangedAttack());
                    break;

                case Enemy.EnemyType.Baserush:
                    parent.StartCoroutine(RushAttack());
                    break;

                case Enemy.EnemyType.BaseAOE:
                    parent.StartCoroutine(AOEAttack());
                    break;
            }
        }

        if (parent.MyTarget != null)
        {
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
        parent._prefabs.PlayAnimation(4);

        yield return new WaitForSeconds(0.15f); // 애니메이션 내려찍기 시작
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/BaseMelee_Attack") as GameObject, parent.exitPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.15f); // 애니메이션 종료
        
        parent.IsAttacking = false;
    }
    
    public IEnumerator rangedAttack()
    {
        parent._prefabs.PlayAnimation(5);

        yield return new WaitForSeconds(0.2f); 
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/BaseRanged_Attack") as GameObject, parent.exitPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f); 
        
        parent.IsAttacking = false;
    }

    public IEnumerator RushAttack()
    {
        parent._prefabs.PlayAnimation(4);
        yield return new WaitForSeconds(1f);        //선딜

        parent.IsRushing = true;
        parent.gameObject.layer = 7;                // Rushing레이어로 바꾸기, 플레이어와 충돌무시
        parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;
        parent.RushSpeed = 7f;
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/BaseRush_Attack") as GameObject, parent.exitPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f); 

        parent.IsRushing = false;
        parent.gameObject.layer = 6;
        parent.IsAttacking = false;

    }

    public IEnumerator AOEAttack()
    {
        parent._prefabs.PlayAnimation(6);
        yield return new WaitForSeconds(1f);
        parent.EnemyAttackResource(Resources.Load("EnemyAttack/BaseAOE_Attack") as GameObject, parent.MyTarget.position, Quaternion.identity);
        parent.IsAttacking = false;
    }
}
