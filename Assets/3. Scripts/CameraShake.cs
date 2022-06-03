using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float ShakeAmount;
    private float ShakeTime;
    private Vector3 InitialPosition;

    private void Start()
    {
        InitialPosition = transform.position;
    }

    public void VibrateForTime(float time)
    {
        ShakeTime = time;
    }
}
