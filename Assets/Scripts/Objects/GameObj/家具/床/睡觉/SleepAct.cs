using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 睡觉的活动
/// </summary>
public class SleepA : Act
{
    public int time = 1;
    public SleepA(Person person,BedObj obj,int time=3):base(person,obj)
    {
        wastTime = true;
        this.time = time;
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        TC();
        Debug.Log("Sleep");
        Act act = null;
        float random = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(random);
        if (random<0.2f)
        {
            yield return GameArchitect.gameLogic.AddDecision(Person,new DecisionTex("ni hao shi jie","sfhsukdfssdjkfhsjkdfhsdf",new List<CardInf>() {
                new CardInf("dadsad","fhsdfjkdgjg",()=>{ Debug.Log("应该成功了"); }) 
            }));
        }
        if (time > 1)
        {
            act = new SleepA(Person, (BedObj)Obj, time - 1);
        }
        else
        {
            act = new EndAct(Person, Obj);
        }
        yield return Ret(act,callback);
    }
}
/// <summary>
/// 睡眠的行为
/// </summary>
[Act]
public class SleepAct : Activity
{
    public int use;
    public SleepAct(Func<Obj, Person, int,object[],bool> cond=null,Func<Obj, Person, int, object[], Act> eff=null):base(cond,eff)
    {
        use = 0;
        activityName = "Sleep";
        detail = "Get Enough Sleep";
    }
    public override bool Condition(Obj obj, Person person,int time, params object[] objs)
    {
        var data = (BedObj)obj;
        if (cond != null)
            return cond(obj,person,time,objs);
        else
            return use<obj.objSaver.size;
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
        var ret=new List<int>();
        for(int i=1;i<=24;i+=1)
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
    public override Act Effect(Obj obj, Person person,int time, params object[] objs)
    {
        return GetActs(new SleepA(person, (BedObj)obj, time), obj, person, time, objs);
    }
}