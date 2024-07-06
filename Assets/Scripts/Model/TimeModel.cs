using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class TimeUpdateEvent
{
    public int Time;
    public TimeUpdateEvent(int time)
    {
        Time = time;
    }
}

public class TimeModel : AbstractModel
{
    public static int timeStep=48;
    /// <summary>
    /// 表示一天24小时
    /// </summary>
    public Num Time;
    protected override void OnInit()
    {
        Time = new Num(new NowT());
        Time.val = GameArchitect.get.tableAsset.tableSaver.time;
    }
    public void AddTime()
    {
        Time.val++;
        this.SendEvent<TimeUpdateEvent>(new TimeUpdateEvent(Time.val));
    }
    public static int GetHours(int time)
    {
        return time % timeStep;
    }
    public int GetTime()
    {
        return Time.val;
    }
    public string GetTimeStr()
    {
        return Time.val / timeStep + "/" + Time.val % timeStep;
    }
    public string GetTimeHour(int time)
    {
        int s=time % timeStep;
        return s/2 + ":" + ((s%2==0)?"00-30":"30-60");
    }
}
