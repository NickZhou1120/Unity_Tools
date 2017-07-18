using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class QualityMgr
{
    //省电模式
    private static int powerMode = -1;//0关闭，1开启
    //高分辨率模式
    private static int highResolution = -1; //0关闭，1开启
    public static int HighResolution 
    {
        get { return highResolution; }
    }
    public static int PowerMode 
    {
        get { return powerMode; }
    }
}
