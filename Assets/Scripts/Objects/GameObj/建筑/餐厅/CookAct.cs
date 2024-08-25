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
    public Int time;
    public CookA(PersonObj PersonObj, BuildingObj obj,Obj selObj, Int time) : base(PersonObj, obj)
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
            yield return Ret(new EndAct(PersonObj, Obj), callback);//����
        }
        else
            yield return Ret(new CookA(PersonObj, buildingObj, selObj,time), callback);//����
    }
}
/// <summary>
/// ѡ����;߿�ʼ�
/// </summary>
public class CookSelA : Act
{
    public int time = 3;
    public CookSelA(PersonObj PersonObj, Obj obj, int priority =10) : base(PersonObj, obj)
    {
        this.time = time;
        wastTime = true;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        BuildingObj building = (BuildingObj)Obj;
        Debug.Log("���");
        List<CardInf> selects = new List<CardInf>();
        Obj selObj = null;
        foreach (var data in building.rates[TransationEnum.cook].ObjList())//ѡ��һϵ�еĲ;�
        {
            var obj = Map.Instance.GetObj(data.Key);
            selects.Add(//�����
                new CardInf(obj.objSaver.title, obj.objSaver.description + ":" + obj.objSaver.TransCount(TransationEnum.cook),
                () =>
                {
                    selObj = obj;
                }
                )
                );
        }
        yield return AddDecision(PersonObj, new DecisionTex("ѡ��;�", "ѡ����;߿�ʼ�",
            selects
        ));///ѡ��һ�����ʵĻ

        if (selObj != null)
        {
            building.rates[TransationEnum.cook].AddRate(selObj);
            var seleA = new SelectTime(PersonObj, selObj,new int[]{ 1,2,3,4,5,6,7,8});
            yield return Ret(
                new SeqAct(PersonObj,Obj,
                    seleA,
                    new CookA(PersonObj,building, selObj, seleA.selectTime)
                ),callback);//����
        }
        else
        {
            yield return Ret(new EndAct(PersonObj, Obj), callback);//�����
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
    public CookAct() : base()
    {
        activityName = "���";
        detail = "��⿶���";
    }
    public override Act Effect(Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs(new CookSelA(PersonObj, obj), obj, PersonObj,winDatas,objs); ;
    }
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
    {
        return true;  
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}
}
