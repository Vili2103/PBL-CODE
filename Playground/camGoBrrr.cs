using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class camGoBrrr : MonoBehaviour
{
    public static camGoBrrr Instance {get; private set;}
    private CinemachineVirtualCamera vcam;
    private float shakeTimer;
    private void Awake() {
        Instance = this;
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void shakeCamera(float intensity, float time) {
        CinemachineBasicMultiChannelPerlin perlin = 
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update() {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0f) {
                CinemachineBasicMultiChannelPerlin perlin = 
                vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                perlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
