using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuyThingsA:Act
{
    Obj goods;
    //要购买的商品
    public BuyThingsA(Person person,Obj obj,Obj goods):base(person,obj)
    {
        this.goods = goods;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("BuyThing");
        yield return Ret(new EndAct(Person,Obj),callback);//返回对象
    }
}
/// <summary>
/// 睡眠的行为
/// </summary>
[Act]
public class BuyThingAct : Activity
{
    public BuyThingAct()
    {
        activityName = "购买物品";
        detail = "购买物品";
    }
    public override bool Condition(Obj obj, Person person,int time,params object[] objs)
    {
        var data = (BedObj)obj;
        return data.capacity.val > data.nowCapacity.val;
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
    /// 选出一系列的活动
    /// </summary>
    /// <param name="person"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override List<CardInf> OutputSelect(Person person, Obj obj)
    {
        var ret = new List<CardInf>();
        for (int i = 1; i <= 1; i++)
        {
            var x = new CardInf(GetType().Name, detail,
            () =>
            {
                person.SetAct(Effect(obj, person, i));
            });
            ret.Add(x);
        }
        return ret;
    }
    /// <summary>
    /// 睡觉效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person, int time, params object[] objs)
    {
        return new SleepA(person, (BedObj)obj, (int)objs[0]);
    }
}