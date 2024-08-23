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
        foreach (var t in buildingObj.goodsManager.goods)
        {
            selects.Add(//可以卖的东西
                new SelectInf(t.Key.sellO.ToString()+ "->" + t.Key.buyO.ToString(), t.Value + "", t.Key, t.Value)
            );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("购买", "选择要购买的东西",
            selects,
            () =>
            {
                DicInt<Goods> resource = new DicInt<Goods>();
                for(int i=0;i<selects.Count;i++)
                {
                    var sx=(Goods)selects[i].obj;
                    var num=selects[i].num;
                    resource.Add(sx,num);
                }
                return GameArchitect.get.GetModel<EcModel>().TryEc(resource,buildingObj.goodsManager,Person.resource);
            }));
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}
/// <summary>
/// 交易行为
/// </summary>
public class BuyAct : Activity
{
    public BuyAct() : base()
    {
        activityName = "买";
        detail = "购买物品";
    }

    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs( new BuyA(person, obj), obj, person,winDatas,objs); ;
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return true;// ((BuildingObj)obj).remainBuilder == 0;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}
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
        foreach(var t in buildingObj.goodsManager.goods)
        {
            selects.Add(//将物品添加到卖场
                new SelectInf(t.Key.sellO.ToString()+"->"+ t.Key.buyO.ToString(),t.Value+"",t.Key,t.Value)
            );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("卖", "选择要卖的东西",
            selects,
            () =>
            {
                foreach (var t in selects)
                {
                    var x=(Obj)t.obj;
                    buildingObj.goodsManager.Add(x.GetEnum(),1,ObjEnum.MoneyObjE,10, t.num);
                }
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
    public SellAct() : base()
    {
        activityName = "卖";
        detail = "卖物品";
    }
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs( new SellA(person, obj), obj, person, winDatas, objs);
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return true;// ((BuildingObj)obj).remainBuilder == 0;
    }
    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}
}
