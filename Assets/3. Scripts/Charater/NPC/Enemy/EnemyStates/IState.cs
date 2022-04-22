using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public interface IState
{
    //Prepare the State
    void Enter(EnemyBase parent);


    void Exit();

    void Update();
}
