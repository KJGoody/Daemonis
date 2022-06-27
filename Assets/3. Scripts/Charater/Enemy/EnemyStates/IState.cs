using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public interface IState
{
    void Enter(EnemyBase parent);

    void Exit();

    void Update();
}
