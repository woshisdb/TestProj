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
    public IdleA(PersonObj PersonObj, Obj obj, int time = 1) : base(PersonObj, obj)
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
            act = new IdleA(PersonObj, Obj, time - 1);
        }
        else
        {
            act = new EndAct(PersonObj, Obj);
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
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
    {
        return true;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    //PersonObj_PDDL PersonObj = new PersonObj_PDDL(new PersonObjType());
    //    return action;
    //}
    /// <summary>
    /// 睡觉效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="PersonObj"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs( new SeqAct(PersonObj, obj,
                new IdleA(PersonObj, obj,1)
               ),obj,PersonObj,winDatas,objs);
    }
}
