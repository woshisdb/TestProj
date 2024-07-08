using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Desk.cs
public class DeskType : ObjType
{
    public DeskType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class DeskSaver : ObjSaver
{

}
[Map()]
// DeskObj.cs
public class DeskObj : Obj
{
    public DeskObj(ObjSaver objAsset=null) : base(objAsset)
    {
        Init();
    }
    public void Init()
    {

    }
    public override List<Activity> InitActivities()
    {
        var acts= base.InitActivities();
        acts.Add(new ArrangeContractAct());//达成协议
        acts.Add(new AddContractAct());//添加协议
        acts.Add(new RemoveContractAct());//移除协议
        return acts;
    }
    public DeskSaver GetSaver()
    {
        return (DeskSaver)objSaver;
    }
}