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
public class PassTime
{
    public virtual int NowTime()
    {
        return 1;
    }
}
public class PassDay: PassTime
{
	public override int NowTime()
	{
        return GameArchitect.get.GetModel<TimeModel>().GetDay();
	}
}

public class PassMonth: PassTime
{
    public override int NowTime()
    {
        return GameArchitect.get.GetModel<TimeModel>().GetMonth();
    }
}

public class PassYear: PassTime
{
    public override int NowTime()
    {
        return GameArchitect.get.GetModel<TimeModel>().GetYear();
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
    public int GetBeginDay()
    {
        return Time.val-Time.val % timeStep;
    }
    public int GetBeginWeek()
    {
        return Time.val - Time.val % (timeStep * 7);
    }
    public int GetBeginMonth()
    {
        return Time.val-Time.val%(timeStep * 30);
    }
    public int GetBeginYear()
    {
        return Time.val - Time.val % (timeStep * 360);
    }
    public int GetDay()
    {
        return Time.val / timeStep;
    }
    public int GetWeek()
    {
        return (Time.val / timeStep)%7;
    }
    public int GetMonth()
    {
        return (Time.val / timeStep)/30;
    }
    public int GetYear()
    {
        return (Time.val / timeStep) / 360;
    }
    ///...............................
    public int GetTime(int t)
    {
        return t;
    }
    public int GetDay(int t)
    {
        return t / timeStep;
    }
    public int GetWeek(int t)
    {
        return (t / timeStep) % 7;
    }
    public int GetMonth(int t)
    {
        return (t / timeStep) / 30;
    }
    public int GetYear(int t)
    {
        return (t / timeStep) / 360;
    }
    //................................
    public string GetTimeStr()
    {
        return Time.val / timeStep + "/" + Time.val % timeStep;
    }
    public string GetTimeHour(int time)
    {
        int s=time % timeStep;
        return s/2 + ":" + ((s%2==0)?"00-30":"30-60");
    }
    public int NextDay(int day)
    {
        return Time.val + timeStep * day;
    }
}
