using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
//using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 睡觉的活动
/// </summary>
public class SleepA : Act
{
    public Int time;
    public SleepA(PersonObj PersonObj,Obj obj, Int time):base(PersonObj,obj)
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
        //float random = UnityEngine.Random.Range(0f, 1f);
        //Debug.Log(random);
        //if (random<0.2f)
        //{
        //    yield return GameArchitect.gameLogic.AddDecision(PersonObj,new DecisionTex("ni hao shi jie","sfhsukdfssdjkfhsjkdfhsdf",new List<CardInf>() {
        //        new CardInf("dadsad","fhsdfjkdgjg",()=>{ Debug.Log("应该成功了"); }) 
        //    }));
        //}
        if (time.val > 1)
        {
            act = new SleepA(PersonObj,Obj, time.val - 1);
        }
        else
        {
            Debug.Log("End");
            act = new EndAct(PersonObj, Obj);
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
    public SleepAct():base()
    {
        use = 0;
        activityName = "睡觉";
        detail = "睡一段时间";
    }
    public override bool Condition(Obj obj, PersonObj PersonObj,params object[] objs)
    {
        return true;
    }
    
    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
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
        var selectTime = new SelectTime(PersonObj, obj, new int[] { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24 });
        return GetActs(
            new SeqAct(PersonObj,obj,
            selectTime,
            new SleepA(PersonObj,obj,selectTime.selectTime)), obj, PersonObj,winDatas,objs);
    }
}