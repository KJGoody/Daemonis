using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PatrolState : IState
{
    private EnemyBase parent;
    private Vector3 PatrolPoint;

    private RaycastHit2D hitTop;
    private RaycastHit2D hitBottom;

    public void Enter(EnemyBase parent)
    {
        this.parent = parent;
            
        do
        {
            while (true)
            {
                float randomNumX = Random.Range(-3f, 3f);
                float randomNumY = Random.Range(-3f, 3f);
                PatrolPoint = new Vector3(parent.myStartPosition.x + randomNumX, parent.myStartPosition.y + randomNumY, parent.myStartPosition.z);

                float radius = (PatrolPoint - parent.transform.position).sqrMagnitude;
                if (radius < 3f * 3f)
                    break;
            }

            hitTop = Physics2D.Raycast(parent.transform.position, PatrolPoint + new Vector3(0, 0.75f, 0), 3f, LayerMask.GetMask("Wall"));
            hitBottom = Physics2D.Raycast(parent.transform.position, PatrolPoint, 3f, LayerMask.GetMask("Wall"));
        } while (hitTop.collider != null && hitBottom.collider != null);



    }

    public void Exit()
    {

    }

    public void Update()
    {

        parent.Direction = PatrolPoint - parent.transform.position;
        float distance = Vector2.Distance(parent.transform.position, PatrolPoint);
        if (distance <= 0.5f)
        {
            parent.ChangeState(new IdleState());
        }

        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    }

    
}
