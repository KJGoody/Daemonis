using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private static CinemachineShake instance;
    public static CinemachineShake Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<CinemachineShake>();
            return instance;
        }
    }

    private CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private float StartingIntensity;
    private float ShakeTimer;
    private float CurrentTime;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if(CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(0f, StartingIntensity, CurrentTime / ShakeTimer);
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        StartingIntensity = intensity;
        ShakeTimer = time;
        CurrentTime = time;
    }

}
