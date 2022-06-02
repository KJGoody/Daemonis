using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puff : MonoBehaviour
{
    [SerializeField]
    private PuffPool.PuffPrefabsName prefabsName;
    [SerializeField]
    private float PuffLifeTime;

    private void OnEnable()
    {
        if(PuffLifeTime != 0)
            StartCoroutine(ReturnPuffObject());
    }

    private IEnumerator ReturnPuffObject()
    {
        yield return new WaitForSeconds(PuffLifeTime);
        PuffPool.Instance.ReturnObject(this, prefabsName);
    }

    public void PositioningPuff(Vector3 Position)
    {
        transform.position = Position;
    }

    public void PositioningPuff(Transform Position)
    {
        transform.SetParent(Position);
        transform.position = Position.position;
    }
}
