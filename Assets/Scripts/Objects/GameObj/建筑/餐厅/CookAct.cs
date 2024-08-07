using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 做饭的行为
/// </summary>
public class CookA:Act
{
    public BuildingObj buildingObj;
    public Obj selObj;
    public Int time;
    public CookA(Person person, BuildingObj obj,Obj selObj, Int time, int priority = -1) : base(person, obj, priority)
    {
        this.time = time;
        wastTime = true;
        this.buildingObj = obj;
        this.selObj = selObj;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        time--;
        if (time == 0)
        {
            buildingObj.rates[TransationEnum.cook].RedRate(selObj);
            yield return Ret(new EndAct(Person, Obj), callback);//返回
        }
        else
            yield return Ret(new CookA(Person, buildingObj, selObj,time, priority), callback);//做饭
    }
}
/// <summary>
/// 选择个餐具开始活动
/// </summary>
public class CookSelA : Act
{
    public int time = 3;
    public CookSelA(Person person, Obj obj, int priority =10,int time=3) : base(person, obj, priority)
    {
        this.time = time;
        wastTime = true;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        BuildingObj building = (BuildingObj)Obj;
        Debug.Log("烹饪");
        List<CardInf> selects = new List<CardInf>();
        Obj selObj = null;
        //building.CookRate.objList.resources = new Dictionary<Obj, int>();
        foreach (var data in building.rates[TransationEnum.cook].ObjList())//选择一系列的餐具
        {
            var obj = Map.Instance.GetObj(data.Key);
            selects.Add(//活动描述
                new CardInf(obj.objSaver.title, obj.objSaver.description + ":" + obj.objSaver.TransCount(TransationEnum.cook),
                () =>
                {
                    selObj = obj;
                }
                )
                );
        }
        yield return AddDecision(Person, new DecisionTex("选择餐具", "选择个餐具开始活动",
            selects
        ));///选择一个合适的活动

        if (selObj != null)
        {
            building.rates[TransationEnum.cook].AddRate(selObj);
            var seleA = new SelectTime(Person, selObj,new int[]{ 1,2,3,4,5,6,7,8});
            yield return Ret(
                new SeqAct(Person,Obj,
                    seleA,
                    new CookA(Person,building, selObj, seleA.selectTime, - 1)
                ),callback);//做饭
        }
        else
        {
            yield return Ret(new EndAct(Person, Obj), callback);//结束活动
        }
    }
}




















public class CookAct : Activity
{
    /// <summary>
    /// 重载条件，效果前的效果
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="eff"></param>
    public CookAct(Func<Obj, Person,object[], bool> cond = null, Func<Obj, Person, object[], Act> eff = null) : base(cond, eff)
    {
        activityName = "烹饪";
        detail = "烹饪东西";
    }
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs(new CookSelA(person, obj), obj, person,winDatas,objs); ;
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return true;  
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
}
