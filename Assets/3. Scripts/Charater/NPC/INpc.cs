using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INpc
{
    public void SetTarget(Transform target);
    public Transform Select();
    public void DeSelect();
}
