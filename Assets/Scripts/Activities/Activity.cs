using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public abstract class Activity
{
    public Func<Obj, Person, int, object[], bool> cond;
    public Func<Obj, Person, int, object[], Act> eff;
    public Activity(Func<Obj, Person, int, object[], bool> cond = null, Func<Obj, Person, int, object[], Act> eff = null)
    {
        this.cond = cond;
        this.eff = eff;
    }
    [SerializeField]
    public string activityName;
    [SerializeField]
    public string detail;
    public virtual List<int> AllowsTime()
    {
        return new List<int>() { 1};
    }
    public virtual bool Condition(Obj obj,Person person,int time, params object[] objs)
    {
        return true;
    }
    public abstract Act Effect(Obj obj, Person person, int time,params object[] objs);
    /// <summary>
    /// 获得当前行为的PDDL形式
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <returns></returns>
    public abstract PAction GetAction();
    /// <summary>
    /// 输出UI的Select
    /// </summary>
    /// <param name="person"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public virtual List<CardInf> OutputSelect(Person person, Obj obj)
    {
        var ret = new List<CardInf>();
        foreach (int i in AllowsTime())
        {
            if (Condition(obj, person, i))
            {
                var x = new CardInf(activityName, "Do " + i + " hours",
                () =>
                {
                    person.SetAct(Effect(obj, person, i));
                });
                ret.Add(x);
            }
        }
        return ret;
    }
    public Act GetActs(Act act, Obj obj, Person person, int time, params object[] objs)
    {
        if (eff != null)
        {
            return new SeqAct(person, obj, eff(obj, person, time, objs),
                act
                );
        }
        return act;
    }
}
