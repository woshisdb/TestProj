using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedType:ObjType
{
    
}

[System.Serializable]
public class BedSaver:ObjSaver
{
    public int BedSit;//˯����λ��
}
/// <summary>
/// ֻ�ܴ�������һ��obj����
/// </summary>
[Map(),Class]
public class BedObj : Obj
{
    public BedObj(ObjSaver objAsset=null) : base(objAsset)
    {
    }
    public override List<Activity> InitActivities()
    {
        List<Activity> activities = new List<Activity>(base.InitActivities());
        activities.Add(new SleepAct());
        return activities;
    }
    public BedSaver GetSaver()
    {
        return (BedSaver)objSaver;
    }
}