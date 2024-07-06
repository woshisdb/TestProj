using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BedType : ObjType
{
    public BedType(string name = null) : base(name)
    {

    }
}

[P]
public class Capacity : Func
{
    public Capacity(ObjType objType) : base(objType)
    {

    }
}
[P]
public class NowCapacity : Func
{
    public NowCapacity(ObjType objType) : base(objType)
    {

    }
}
[System.Serializable]
public class BedSaver:ObjSaver
{
    public int BedSit;//睡觉的位置
}
/// <summary>
/// 只能存在于另一个obj里面
/// </summary>
[Map()]
public class BedObj : Obj<BedSaver>
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
}