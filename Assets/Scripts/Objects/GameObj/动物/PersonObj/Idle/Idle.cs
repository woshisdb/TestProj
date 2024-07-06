using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 什么都不干的活动
/// </summary>
public class IdleA : Act
{
    public int time = 1;
    public IdleA(Person person, Obj obj, int time = 1) : base(person, obj)
    {
        wastTime = true;
        this.time = time;
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        TC();
        Debug.Log("Idle");
        Act act = null;
        if (time > 1)
        {
            act = new IdleA(Person, Obj, time - 1);
        }
        else
        {
            act = new EndAct(Person, Obj);
        }
        yield return Ret(act, callback);
    }
}

[Act]
public class IdleAct : Activity
{
    public IdleAct()
    {
        activityName = "Idle";
        detail = "IdleTime";
    }
    public override bool Condition(Obj obj, Person person, int time, params object[] objs)
    {
        return true;
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
    [Button]
    public void ShowAction()
    {
        Debug.Log(GetAction().ToString());
    }
    public override List<int> AllowsTime()
    {
        var ret = new List<int>();
        for (int i = 1; i <= 24; i += 1)
            ret.Add(i);
        return ret;
    }
    /// <summary>
    /// 睡觉效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person, int time, params object[] objs)
    {
        return new SeqAct(person, obj,
                new IdleA(person, obj, time)
               );
    }
}
