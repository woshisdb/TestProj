using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoA:Act
{
    TableModel x;
    TableModel y;
    public GoA(Person person,Obj obj,TableModel x,TableModel y):base(person,obj)
    {
        wastTime = true;
        this.x = x;
        this.y = y;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("LeaveA2B");
        x.LeaveTable(Person);
        y.EnterTable(Person);
        yield return Ret(new EndAct(Person,Obj), callback);
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

    public override CardInf OutputSelect(Person person, Obj obj)
    {
        Init();
        return new CardInf("Go Home","Waste 15m", () => {
            person.SetAct(
                new GoA(person, obj, x, y)
            );
        }
        );
    }

    public override Act Effect(Obj obj, Person person,params object[] objs)
    {
        return new GoA(person, obj, x, y);
    }

    public Go(TableModel x, TableModel y, int wasteTime)
    {
        this.x = x;
        this.y = y;
        this.wasteTime = wasteTime;
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
        return GameArchitect.get.tableAsset.tableSaver.activities; 
    }
    public PathSaver GetSaver()
    {
        return (PathSaver)objSaver;
    }
}
///// <summary>
///// 从Man中涉及的道路
///// </summary>
//public class ManPanelPathType : PathType
//{
//    public ManPanelPathType(string name = null) : base(name)
//    {

//    }
//}
//[Map()]
//public class ManPanelPathObj : PathObj
//{
//    public ManPanelPathObj(PathSaver objSaver) :base(objSaver)
//    {
//        Init();
//    }
//    public void Init()
//    {
//        cardInf.title = "Path";
//        cardInf.description = "GoHomePath";
//    }
//    public override List<Activity> InitActivities()
//    {
//        List<Activity> activities = new List<Activity>();
//        activities.Add(new Go("MainPanel","Home",1));//花费1个小时，一天24小时
//        return activities;
//    }
//}
///// <summary>
///// Home中的道路
///// </summary>
//public class HomePathType:PathType
//{
//    public HomePathType(string name = null) : base(name)
//    {

//    }
//}
//[Map()]
//public class HomePathObj: PathObj
//{
//    public HomePathObj(PathSaver objSaver) : base(objSaver)
//    {
//        Init();
//    }
//    public void Init()
//    {
//        cardInf.title="Path";
//        cardInf.description="GoMainPath";
//    }
//    public override List<Activity> InitActivities()
//    {
//        List<Activity> activities = new List<Activity>();
//        activities.Add(new Go("Home", "MainPanel", 1));
//        return activities;
//    }
//}