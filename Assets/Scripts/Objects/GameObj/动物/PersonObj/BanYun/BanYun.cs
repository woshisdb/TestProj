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
    public BanYunA(PersonObj PersonObj, Obj obj) : base(PersonObj, obj)
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
        int remainSize = (PersonObj.resource.maxSize - PersonObj.resource.nowSize)/ Map.Instance.GetSaver(selObj).size;
        remainSize = Mathf.Min(remainSize, building.resource.resources[selObj].remain);
        Resource.Trans(selObj,remainSize, building.resource, PersonObj.resource);
        yield return Ret(new EndAct(PersonObj, Obj), callback);
    }
}

public class FangZhiA : Act
{
    public int time = 1;
    public FangZhiA(PersonObj PersonObj, Obj obj) : base(PersonObj, obj)
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
        foreach (var x in PersonObj.resource.resources)
        {
            moveList.Add(new CardInf(x.Key.ToString(), "",
            () => { selObj = x.Key; }
            ));
        }
        int remainSize = (building.resource.maxSize - building.resource.nowSize) / Map.Instance.GetSaver(selObj).size;
        remainSize = Mathf.Min(remainSize, PersonObj.resource.resources[selObj].remain);
        Resource.Trans(selObj, remainSize, PersonObj.resource, building.resource);
        yield return Ret(new EndAct(PersonObj, Obj), callback);
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
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
    {
        return true;
    }

    /// <summary>
    /// 睡觉效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="PersonObj"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs( new SeqAct(PersonObj, obj,
                new BanYunA(PersonObj, obj)
               ),obj,PersonObj,winDatas,objs);
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
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
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
        return GetActs(new SeqAct(PersonObj, obj,
                new FangZhiA(PersonObj, obj)
               ), obj, PersonObj, winDatas, objs);
    }
}
