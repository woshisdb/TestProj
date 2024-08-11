using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 什么都不干的活动
/// </summary>
public class BanYunA : Act
{
    public int time = 1;
    public BanYunA(Person person, Obj obj) : base(person, obj)
    {
        wastTime = true;
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        TC();
        Debug.Log("BanYun");
        BuildingObj building = (BuildingObj)Obj;
        var moveList=new List<CardInf>();
        ObjEnum selObj=ObjEnum.ObjE;
        foreach(var x in building.resource.resources)
        {
            moveList.Add(new CardInf(x.Key.ToString(),"",
            () => { selObj = x.Key; }    
            ) );
        }
        int remainSize = (Person.resource.maxSize - Person.resource.nowSize)/ Map.Instance.GetSaver(selObj).size;
        remainSize = Mathf.Min(remainSize, building.resource.resources[selObj].remain);
        Resource.Trans(selObj,remainSize, building.resource, Person.resource);
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}

public class FangZhiA : Act
{
    public int time = 1;
    public FangZhiA(Person person, Obj obj) : base(person, obj)
    {
        wastTime = true;
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        TC();
        Debug.Log("FangZhi");
        BuildingObj building = (BuildingObj)Obj;
        var moveList = new List<CardInf>();
        ObjEnum selObj = ObjEnum.ObjE;
        foreach (var x in Person.resource.resources)
        {
            moveList.Add(new CardInf(x.Key.ToString(), "",
            () => { selObj = x.Key; }
            ));
        }
        int remainSize = (building.resource.maxSize - building.resource.nowSize) / Map.Instance.GetSaver(selObj).size;
        remainSize = Mathf.Min(remainSize, Person.resource.resources[selObj].remain);
        Resource.Trans(selObj, remainSize, Person.resource, building.resource);
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}

[Act]
public class BanYunAct : Activity
{
    public BanYunAct()
    {
        activityName = "BanYun";
        detail = "BanYunTime";
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return true;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}
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
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs( new SeqAct(person, obj,
                new BanYunA(person, obj)
               ),obj,person,winDatas,objs);
    }
}

[Act]
public class FangZhiAct : Activity
{
    public FangZhiAct()
    {
        activityName = "FangZhi";
        detail = "FangZhiTime";
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return true;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}
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
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs(new SeqAct(person, obj,
                new FangZhiA(person, obj)
               ), obj, person, winDatas, objs);
    }
}
