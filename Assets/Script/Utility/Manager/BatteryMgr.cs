using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 优化电量和发热
/// 1、对于分辨率过高的手机，降低分辨率(高清模式除外)
/// 2、空闲超过一定时间，锁定30帧(如已经打开省电模式，则不用做此检测)
/// </summary>
public class BatteryMgr : MonoBehaviour
{

    //优化帧率开关
#if UNITY_IPHONE || UNITY_ANDROID   
    public static bool Optimize = true;
#else
    public static bool Optimize = true;
#endif

    //最大分辨率设置
#if UNITY_IPHONE    
    public static int maxWidth = 1920;
#elif UNITY_ANDROID
    public static int maxWidth = 1280;
#else
    public static int maxWidth = 1920;
#endif

    //空闲秒数
    private static int maxIdleTime = 120;
    private static float idleTime = 0;
    private static int curFrameRate = 0;


    private static int origWidth = 0;
    private static int origHeight = 0;

    private const int hightFrameRate = 60;
    private const int lowFrameRate = 30;

    void Awake()
    {
        origWidth = Screen.width;
        origHeight = Screen.height;

        if (QualityMgr.HighResolution == 0)
        {
            ChangeResolution(maxWidth);
        }
    }

    void Update()
    {
        if (!Optimize || QualityMgr.PowerMode == 1)
            return;
#if UNITY_EDITOR
        bool input = Input.GetMouseButton(0);

#elif UNITY_IPHONE ||UNITY_ANDROID        
        input = Input.touchCount > 0;
#else        
        input = true;
#endif

        if (input || Util.IsPlaying)
        {
            if (curFrameRate != hightFrameRate)
            {
                Reset();
            }
        }
        else
        {
            idleTime += Time.deltaTime;
            if (idleTime > maxIdleTime)
            {
                if (curFrameRate != lowFrameRate)
                {
                    curFrameRate = lowFrameRate;
                    Application.targetFrameRate = lowFrameRate;
                }
            }
        }
    }

    void ChangeResolution(bool toHigh)
    {
        if (toHigh)
        {
            ChangeResolution(origWidth);
        }
        else
        {
            ChangeResolution(maxWidth);
        }

    }
    void ChangeResolution(int width)
    {
        if (width > 0 && origWidth >= width)
        {
            float ratio = (float)Screen.height / Screen.width;
            int height = (int)(width * ratio);
            if (height % 2 == 1)
            {
                Debug.Log("Could not change resolution");
            }
            else
            {
                Screen.SetResolution(width, height, true);
            }
        }
    }


    void Reset()
    {
        if (!Optimize || QualityMgr.PowerMode == 1)
        {
            return;
        }
        idleTime = 0;
        curFrameRate = hightFrameRate;
        Application.targetFrameRate = hightFrameRate;
    }
}
