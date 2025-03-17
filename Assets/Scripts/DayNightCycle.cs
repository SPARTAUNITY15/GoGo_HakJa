using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // �Ϸ縦 1�� �����ϰ�, �������� �ð�, fullDayLength �Ϸ��� ����(seconds), timeRate�� 1�ʸ��� ������ �Ϸ��� ����.
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

        //ȯ�汤, �ݻ籤 ����. ȯ�汤: ������(���籤�� ���� �ʴ� �κ��� ���), �ݻ籤: ������ ���� �ݻ�ȿ���� ����
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

        Transition();
    }

    void UpdateLighting(Light lightSource, Gradient colorrGradiant, AnimationCurve intensityCurve)
    {
        //������ � ����ְ�, x�� time, y�� ������� �޾Ƽ� �װ� intensity�� �־��ִ°ų�

        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorrGradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        // activeInHierarchy. activeSelf ����
        // Parent (��Ȱ��ȭ��)
        //������ Child(Ȱ��ȭ��)
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
            // ����
            isTransiting = true;

        }
        else if (prevTime < 0.65f && time > 0.65f)
        {
            // �ϸ�
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
