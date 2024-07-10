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
    public Int time;
    public SleepA(Person person,Obj obj, Int time):base(person,obj)
    {
        wastTime = true;
        this.time =time;
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        Debug.Log(this.time.val);
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
        if (time.val > 1)
        {
            act = new SleepA(Person,Obj, time.val - 1);
        }
        else
        {
            Debug.Log("End");
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
    public SleepAct(Func<Obj, Person, object[],bool> cond=null,Func<Obj, Person, object[], Act> eff=null):base(cond,eff)
    {
        use = 0;
        activityName = "睡觉";
        detail = "睡一段时间";
    }
    public override bool Condition(Obj obj, Person person,params object[] objs)
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
    /// <summary>
    /// 睡觉效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person,params object[] objs)
    {
        return GetActs(new SleepA(person,obj,3), obj, person,objs);
    }
}