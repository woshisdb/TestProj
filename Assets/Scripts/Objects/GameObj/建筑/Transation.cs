using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 购买物品
/// </summary>
public class BuyA : Act
{
    public BuyA(Person person, Obj obj, int priority = -1) : base(person, obj, priority)
    {
        wastTime = true;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("交易");
        List<SelectInf> selects = new List<SelectInf>();
        BuildingObj buildingObj = (BuildingObj)Obj;//建筑对象
        foreach (var t in buildingObj.goodsManager.items)
        {
            selects.Add(//可以卖的东西
                new SelectInf(t.Key.obj.objSaver.title, t.Key.obj.objSaver.description+"X"+ t.Key.num+":"+t.Key.cost, t.Key.obj)
            );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("购买", "选择要购买的东西",
            selects,
            () =>
            {
                int sum = 0;
                for(int i=0;i<selects.Count;i++)
                {
                    var sx=(Goods)selects[i].obj;
                    sum += sx.cost * selects[i].num;
                }
                if(sum > Person.money.val)
                {
                    return false;//不能执行
                }
                else
                {
                    Person.money.val -= sum;
                    return true;
                }
            }));
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}
/// <summary>
/// 交易行为
/// </summary>
public class BuyAct : Activity
{
    public BuyAct(Func<Obj, Person,object[], bool> cond=null, Func<Obj, Person,object[], Act> eff = null) : base(cond, eff)
    {
        activityName = "买";
        detail = "购买物品";
    }

    public override Act Effect(Obj obj, Person person,params object[] objs)
    {
        return new BuyA(person, obj);
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
}
/// <summary>
/// 挂到单子上
/// </summary>
public class SellA : Act
{
    public SellA(Person person, Obj obj, int priority = -1) : base(person, obj, priority)
    {
        wastTime = true;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        BuildingObj buildingObj = (BuildingObj)Obj;//建筑对象
        Debug.Log("交易");
        List<SelectInf> selects = new List<SelectInf>();
        foreach(var t in buildingObj.goodsManager.items)
        {
            selects.Add(//将物品添加到卖场
                new SelectInf(t.Key.obj.objSaver.title, t.Key.obj.objSaver.description,t.Key.obj)
            );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("卖", "选择要卖的东西",
            selects,
            () =>
            {
                return true;
            }));
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}
/// <summary>
/// 交易行为
/// </summary>
public class SellAct : Activity
{
    public SellAct(Func<Obj, Person,object[], bool> cond = null, Func<Obj, Person,object[], Act> eff = null) : base(cond, eff)
    {
        activityName = "卖";
        detail = "卖物品";
    }

    public override Act Effect(Obj obj, Person person, params object[] objs)
    {
        return new SellA(person, obj);
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
}
