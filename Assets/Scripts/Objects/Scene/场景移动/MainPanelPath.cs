using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterA:Act
{
    string x;
    public EnterA(Person person, Obj obj, string x) : base(person, obj)
    {
        this.x = x;
        wastTime = false;
    }
    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("Enter");
        GameArchitect.get.GetModel<TableModelSet>().Get(x).EnterTable(Person);
        yield return Ret(new EndAct(Person,Obj), callback);
    }
}
public class LeaveA : Act
{
    string x;
    public LeaveA(Person person, Obj obj, string x) : base(person, obj)
    {
        wastTime = false;
        this.x = x;
    }
    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("LeaveA2B");
        GameArchitect.get.GetModel<TableModelSet>().Get(x).LeaveTable(Person);
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}

public class Go : Activity
{
    public string xname;
    public string yname;
    public int wasteTime;
    public override bool Condition(Obj obj, Person person,params object[] objs)
    {
        return person.belong == GameArchitect.get.GetModel<TableModelSet>().Get(xname);
    }
	public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return new SeqAct(person, obj,
            new LeaveA(person, obj, xname),
            new WasteTimeA(person, obj, wasteTime),
            new EnterA(person, obj, yname)
        );
    }

    public Go(string x, string y, int wasteTime)
    {
        this.xname = x;
        this.yname = y;
        this.wasteTime = wasteTime;
        activityName = x + "->" + y;
        detail = wasteTime + "(H/2)";
    }
}
public class PathType : ObjType
{
}
[System.Serializable]
public class PathSaver:ObjSaver
{
    public PathSaver()
    {
        size = 1;
    }
}
[Map(),Class]
public class PathObj:Obj
{
    public PathObj(PathSaver objSaver=null) : base(objSaver)
    {
        Init();
    }
    public void Init()
    {

    }
    public override List<Activity> InitActivities()
    {
        return GameArchitect.get.objAsset.map.activities;
    }
    public PathSaver GetSaver()
    {
        return (PathSaver)objSaver;
    }
}
