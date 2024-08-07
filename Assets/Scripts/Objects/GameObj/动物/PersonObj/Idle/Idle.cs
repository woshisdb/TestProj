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
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return true;
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        Person_PDDL person = new Person_PDDL(new PersonType());
        person.money
        return action;
    }
    [Button]
    public void ShowAction()
    {
        Debug.Log(GetAction().ToString());
    }
    /// <summary>
    /// 睡觉效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs( new SeqAct(person, obj,
                new IdleA(person, obj,1)
               ),obj,person,winDatas,objs);
    }
}
