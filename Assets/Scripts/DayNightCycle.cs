using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // 하루를 1로 상정하고, 나머지는 시간, fullDayLength 하루의 길이(seconds), timeRate는 1초만에 지나갈 하루의 길이.
    [Range(0.0f, 1.0f)]
    public float time;
    public float prevTime;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    [Header("SkyBox")]
    public Material daybox;
    public Material nightbox;
    public Material sunSetbox;

    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
        //RenderSettings.skybox = Daybox;
    }

    void Update()
    {
        prevTime = time;
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        //환경광, 반사광 조절. 환경광: 간접광(직사광이 닿지 않는 부분의 밝기), 반사광: 재질에 따른 반사효과의 강도
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

        Transition();
    }

    void UpdateLighting(Light lightSource, Gradient colorrGradiant, AnimationCurve intensityCurve)
    {
        //임의의 곡선 집어넣고, x에 time, y에 결과값을 받아서 그걸 intensity에 넣어주는거네

        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorrGradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        // activeInHierarchy. activeSelf 차이
        // Parent (비활성화됨)
        //└── Child(활성화됨)
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
            //if(lightSource == sun)
            //{
            //    RenderSettings.skybox = Nightbox;
            //}
            //else if (lightSource == moon)
            //{
            //    RenderSettings.skybox = Daybox;
            //}
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);

        }
    }

    public enum SkyState
    {
        day,
        middle,
        night
    }

    bool isTransiting = false;
    void Transition()
    {
        float transTime = fullDayLength / 10;
        if (prevTime < 0.2f && time > 0.2f)
        {
            // 일출
            isTransiting = true;

        }
        else if (prevTime < 0.65f && time > 0.65f)
        {
            // 일몰
            isTransiting = true;
            //LerpSkybox(daybox, )
        }
        else
        {
            isTransiting = false;
            if (time < 0.2f || time > 0.75f)
                RenderSettings.skybox = nightbox;
            else if (time > 0.3f && time < 0.65f)
                RenderSettings.skybox = daybox;
        }
    }

    void UpdateSkybox()
    {
        

    }

    void LerpSkybox(Material from, Material middle, Material to, float t)
    {
        RenderSettings.skybox.Lerp(to, from, t);
    }
}
