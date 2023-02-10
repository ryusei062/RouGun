using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class OverrideLight2D : MonoBehaviour
{
    [SerializeField]
    private Light2D light2d;

    [SerializeField]
    private BonfireManager bonfire;

    private float Timer = 2f;
    private float dawnLTIntensity = 0.02f;
    private float dawnLTInnerRadius = 0.015f;
    private float dawnLTOuterRadius = 0.02f;

    private void Start()
    {
        light2d.intensity = PlayerStatusTakeOver.currentLTIntensity;
        light2d.pointLightInnerRadius = PlayerStatusTakeOver.curretnLTInnerRadius;
        light2d.pointLightOuterRadius = PlayerStatusTakeOver.currentLTOuterRadius;

    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            if (PlayerStatusTakeOver.currentLT <= 0) { return; }
            PlayerStatusTakeOver.currentLTIntensity -= dawnLTIntensity;
            PlayerStatusTakeOver.curretnLTInnerRadius -= dawnLTInnerRadius;
            PlayerStatusTakeOver.currentLTOuterRadius -= dawnLTOuterRadius;
            light2d.intensity = PlayerStatusTakeOver.currentLTIntensity;
            light2d.pointLightInnerRadius = PlayerStatusTakeOver.curretnLTInnerRadius;
            light2d.pointLightOuterRadius = PlayerStatusTakeOver.currentLTOuterRadius;
            Timer = 2f;
        }
     
    }
}