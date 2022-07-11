using UnityEngine;

public class NPC : MonoBehaviour, INpc
{
    public virtual void SetTarget(Transform target)
    {

    }

    public virtual Transform Select()
    {
        return transform;
    }

    public virtual void DeSelect()
    {

    }
}