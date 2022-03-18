using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;

    private Transform target;
    public Transform Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }
    protected override void Update()
    {
        FollowTarget();
        base.Update();
    }
    public override Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;

        return base.Select();
    }


    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }
    private void FollowTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            Vector3 myPosition = transform.position;

            transform.position = Vector2.MoveTowards(myPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}