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
        acts.Add(new ArrangeContractAct());//���Э��
        acts.Add(new AddContractAct());//���Э��
        acts.Add(new RemoveContractAct());//�Ƴ�Э��
        return acts;
    }
    public DeskSaver GetSaver()
    {
        return (DeskSaver)objSaver;
    }
}