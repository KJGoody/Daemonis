using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    private IState currentState;

    [SerializeField]
    private CanvasGroup healthGroup;

    [SerializeField]
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
    protected void Awake()
    {
        ChangeState(new IdleState());
    }
    protected override void Update()
    {
        currentState.Update();
        base.Update();
    }
    protected override void FixedUpdate()
    {
        //FollowTarget();
        base.FixedUpdate();
    }
    public override Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;

        return base.Select();
    }
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }


    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }
    //private void FollowTarget()
    //{
    //    if (target != null)
    //    {
    //        Vector2 targetPosition = target.position;
    //        Vector2 myPosition = transform.position;
    //        direction = targetPosition - myPosition;


    //        //transform.position = Vector2.MoveTowards(myPosition, targetPosition, speed * Time.deltaTime);
    //    }
    //    else{
    //        direction = Vector2.zero;
    //    }
    //}
}