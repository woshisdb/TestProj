using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public abstract class Activity
{
    public Func<Obj, Person, object[], bool> cond;
    public Func<Obj, Person, object[], Act> eff;
    public Activity()
    {
        this.cond = null;
        this.eff = null;
    }
    [SerializeField]
    public string activityName;
    [SerializeField]
    public string detail;
    public virtual bool Condition(Obj obj,Person person, params object[] objs)
    {
        return true;
    }
    public abstract Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs);
    /// <summary>
    /// 获得当前行为的PDDL形式
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <returns></returns>
    public abstract PAction GetAction();

    public virtual List<Predicate> GetPredicates()
    {
        return new List<Predicate>();
    }
    public virtual List<Func> GetFuncs()
    {
        return new List<Func>();
    }
    /// <summary>
    /// 输出UI的Select
    /// </summary>
    /// <param name="person"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public virtual CardInf OutputSelect(Person person, Obj obj)
    {
        var ret = new CardInf(activityName, detail,
            () =>
            {
                person.SetAct(Effect(obj, person));
            });
        return ret;
    }
    public Act GetActs(Act act, Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        if(winDatas!=null)
        {
            act.SetWinData(winDatas);
        }
        if (eff != null)
        {
            return new SeqAct(person, obj, eff(obj, person, objs),
                act
                );
        }
        return act;
    }
    public virtual List<WinData> GetWins()
    {
        return null;
    }

}
