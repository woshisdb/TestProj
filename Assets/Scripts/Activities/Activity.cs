using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public abstract class Activity
{
    public Func<Obj, PersonObj, object[], bool> cond;
    public Func<Obj, PersonObj, object[], Act> eff;
    public Activity()
    {
        this.cond = null;
        this.eff = null;
    }
    [SerializeField]
    public string activityName;
    [SerializeField]
    public string detail;
    public virtual bool Condition(Obj obj,PersonObj PersonObj, params object[] objs)
    {
        return true;
    }
    public abstract Act Effect(Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs);

    /// <summary>
    /// Êä³öUIµÄSelect
    /// </summary>
    /// <param name="PersonObj"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public virtual CardInf OutputSelect(PersonObj PersonObj, Obj obj)
    {
        var ret = new CardInf(activityName, detail,
            () =>
            {
                PersonObj.SetAct(Effect(obj, PersonObj));
            });
        return ret;
    }
    public Act GetActs(Act act, Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs)
    {
        if(winDatas!=null)
        {
            act.SetWinData(winDatas);
        }
        if (eff != null)
        {
            return new SeqAct(PersonObj, obj, eff(obj, PersonObj, objs),
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
