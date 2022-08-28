using UnityEngine;

public class NPC : MonoBehaviour, INpc
{
    [SerializeField] private SPUM_Prefabs CharacterImage;
    protected Transform Target;

    private void Update()
    {
        if (Target != null)
        {
            if ((Target.transform.position - transform.position).x > 0)
                CharacterImage.transform.localScale = new Vector3(-1, 1, 1);
            else if ((Target.transform.position - transform.position).x < 0)
                CharacterImage.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public virtual void SetTarget(Transform target)
    {
        Target = target;
    }
    public virtual Transform Select() { return transform; }
    public virtual void DeSelect() { }
}