using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��������Ϊ
/// </summary>
public class CookA:Act
{
    public BuildingObj buildingObj;
    public Obj selObj;
    public int time;
    public CookA(Person person, BuildingObj obj,Obj selObj, int priority = -1,int time=1) : base(person, obj, priority)
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
            buildingObj.rates[TransationEnum.cook].Release(selObj);
            yield return Ret(new EndAct(Person, Obj), callback);//����
        }
        else
            yield return Ret(new CookA(Person, buildingObj, selObj, priority, time), callback);//����
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
        //building.CookRate.objList.resources = new Dictionary<Obj, int>();
        foreach (var data in building.rates[TransationEnum.cook].objList.resources)//ѡ��һϵ�еĲ;�
        {
            var obj = ((ObjCont)data.Value).obj;
            selects.Add(//�����
                new CardInf(obj.objSaver.title, obj.objSaver.description + ":" + obj.objSaver.canCook.count,
                () =>
                {
                    selObj = obj;
                }
                )
                );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person, new DecisionTex("ѡ��;�", "ѡ����;߿�ʼ�",
            selects
        ));///ѡ��һ�����ʵĻ

        if (selObj != null)
        {
            building.rates[TransationEnum.cook].Use(selObj);
            var seleA = new SelectTime(Person, selObj,new int[]{ 1,2,3,4,5,6,7,8});
            yield return Ret(
                new SeqAct(Person,Obj,
                    seleA,
                    new CookA(Person,building, selObj, -1,seleA.selectTime)
                ),callback);//����
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
    public CookAct(Func<Obj, Person,object[], bool> cond = null, Func<Obj, Person, object[], Act> eff = null) : base(cond, eff)
    {
        activityName = "���";
        detail = "��⿶���";
    }
    public override Act Effect(Obj obj, Person person,params object[] objs)
    {
        return new CookSelA(person, obj);
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
}
