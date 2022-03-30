using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public interface IState
{
    //Prepare the State
    void Enter(Enemy parent);

    void Update();

    void Exit();

}
