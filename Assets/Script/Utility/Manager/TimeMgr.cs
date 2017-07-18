using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMgr : MonoBehaviour
{
    public static TimeMgr Instance;

    private bool enableScaleTime;
    private float scaleTimeStart = 0;
    private float scaleTimeDuration = 0;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        UpdateTimeScale();
    }

    void UpdateTimeScale()
    {
        if (enableScaleTime)
        {
            if ((Time.realtimeSinceStartup - scaleTimeStart) >= scaleTimeDuration)
            {
                Reset();
            }
        }
    }
    void Scale(float factor) 
    {
        Time.timeScale = factor;
    }

    public void Reset() 
    {
        enableScaleTime = false;
        scaleTimeStart = 0;
        scaleTimeDuration = 0;
        Scale(1.0f);
    }
    public void Pause() 
    {
        Reset();
        Scale(0f);
    }
    public void Resume() 
    {
        Scale(1.0f);
    }

    public void TimeScale(float factor, float duration) 
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (enableScaleTime)
            return;
        enableScaleTime = true;
        scaleTimeStart = Time.realtimeSinceStartup;
        scaleTimeDuration = duration;
        Scale(factor);
    }

}
