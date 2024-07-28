using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterA:Act
{
    TableModel x;
    TableModel y;
    public EnterA(Person person,Obj obj,TableModel x):base(person,obj)
    {
        wastTime = false;
        this.x = x;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("Enter");
        y.EnterTable(Person);
        yield return Ret(new EndAct(Person,Obj), callback);
    }
}
public class LeaveA : Act
{
    TableModel x;
    public LeaveA(Person person, Obj obj, TableModel x) : base(person, obj)
    {
        wastTime = false;
        this.x = x;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("LeaveA2B");
        x.LeaveTable(Person);
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}

public class Go : Activity
{
    public string xname;
    public string yname;
    public TableModel x=null;
    public TableModel y=null;
    public int wasteTime;
    public override bool Condition(Obj obj, Person person,params object[] objs)
    {
        Init();
        return person.belong == x;
    }
    public void Init()
    {
        if(x==null)
        x= GameArchitect.get.tableAsset.tableSaver.tables.Find(e => { return e.TableName == xname; });
        if(y==null)
        y = GameArchitect.get.tableAsset.tableSaver.tables.Find(e => { return e.TableName == yname; });
    }
    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }

    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return new SeqAct(person, obj,
            new LeaveA(person, obj, x),
            new WasteTimeA(person, obj, wasteTime),
            new EnterA(person, obj, y)
        );
    }

    public Go(TableModel x, TableModel y, int wasteTime)
    {
        this.x = x;
        this.y = y;
        this.wasteTime = wasteTime;
        activityName = x.TableName + "->" + y.TableName;
        detail = wasteTime+"(H/2)";
    }
    public Go(string x, string y, int wasteTime)
    {
        this.xname = x;
        this.yname = y;
        this.wasteTime = wasteTime;
    }
}
public class PathType:ObjType
{
    public PathType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class PathSaver:ObjSaver
{
    public PathSaver()
    {
        size = 1;
    }
}
[Map()]
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
