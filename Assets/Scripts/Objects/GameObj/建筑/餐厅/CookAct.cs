using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 做饭的行为
/// </summary>
public class CookA:Act
{
    public CookItems items;
    public int time;
    public CookA(Person person, Obj obj,CookItems cookItems,int priority = -1,int time=1) : base(person, obj, priority)
    {
        items = cookItems;
        this.time = time;
        wastTime = true;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        time--;
        items.Cook(Obj.objSaver.canCook.count);
        if (time == 0)
            yield return Ret(new EndAct(Person,Obj),callback);//返回
        else
            yield return Ret(new CookA(Person,Obj, items, priority, time),callback);//做饭
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
        RestaurantObj building = (RestaurantObj)Obj;
        Debug.Log("烹饪");
        List<CardInf> selects = new List<CardInf>();
        Obj selObj = null;
        foreach (var data in building.CookRate.objList.resources)//选择一系列的餐具
        {
            selects.Add(//活动描述
                new CardInf(data.Key.objSaver.title, data.Key.objSaver.description + ":" + data.Key.objSaver.canCook.count,
                () =>
                {
                    selObj = data.Key;
                }
                )
                );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person, new DecisionTex("选择餐具", "选择个餐具开始活动",
            selects
        ));///选择一个合适的活动
        if (selObj != null)
        {
            yield return Ret(new CookA(Person,selObj,building.CookItems),callback);//做饭
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
    public CookAct(Func<Obj, Person, int, object[], bool> cond = null, Func<Obj, Person, int, object[], Act> eff = null) : base(cond, eff)
    {
        activityName = "烹饪";
        detail = "烹饪东西";
    }
    public override Act Effect(Obj obj, Person person, int time, params object[] objs)
    {
        return new CookSelA(person, obj);
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
}
