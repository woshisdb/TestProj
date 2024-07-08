using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��������Ϊ
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
            yield return Ret(new EndAct(Person,Obj),callback);//����
        else
            yield return Ret(new CookA(Person,Obj, items, priority, time),callback);//����
    }
}
/// <summary>
/// ѡ����;߿�ʼ�
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
        Debug.Log("���");
        List<CardInf> selects = new List<CardInf>();
        Obj selObj = null;
        foreach (var data in building.CookRate.objList.resources)//ѡ��һϵ�еĲ;�
        {
            selects.Add(//�����
                new CardInf(data.Key.objSaver.title, data.Key.objSaver.description + ":" + data.Key.objSaver.canCook.count,
                () =>
                {
                    selObj = data.Key;
                }
                )
                );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person, new DecisionTex("ѡ��;�", "ѡ����;߿�ʼ�",
            selects
        ));///ѡ��һ�����ʵĻ
        if (selObj != null)
        {
            yield return Ret(new CookA(Person,selObj,building.CookItems),callback);//����
        }
        else
        {
            yield return Ret(new EndAct(Person, Obj), callback);//�����
        }
    }
}

public class CookAct : Activity
{
    /// <summary>
    /// ����������Ч��ǰ��Ч��
    /// </summary>
    /// <param name="cond"></param>
    /// <param name="eff"></param>
    public CookAct(Func<Obj, Person, int, object[], bool> cond = null, Func<Obj, Person, int, object[], Act> eff = null) : base(cond, eff)
    {
        activityName = "���";
        detail = "��⿶���";
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
