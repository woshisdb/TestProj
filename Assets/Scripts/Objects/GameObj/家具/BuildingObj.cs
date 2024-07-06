using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingType : ObjType
{
    public BuildingType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class BuildingSaver : ObjSaver
{
    /// <summary>
    /// 容量大小
    /// </summary>
    public int container;
}
public class Sit
{
    public int sit;
    public int useSit;
}
public class BuildingObj<T> : Obj<T> where T : BuildingSaver
{
    public Sit BedSit;//床的数目
    public Sit KitchenwareSit;//厨具的数目
    public Sit SetSit;//坐下的数目
    /// <summary>
    /// 容纳的物体
    /// </summary>
    public Dictionary<Obj, int> objs;
    
    public BuildingObj(BuildingSaver objSaver):base(objSaver)
    {
        objs=new Dictionary<Obj, int>();
    }
    public override List<Activity> InitActivities()
    {
        return new List<Activity>() {
        new SleepAct(
            (obj, person, time, objs) => {return BedSit.useSit < BedSit.sit; },
            (obj, person, time, objs) => new SeqAct(person, obj, new SleepA(person, (BedObj)obj, time))
        ),
        new ArrangeContractAct(),
        new AddContractAct(),
        new RemoveContractAct()
    };
    }
    public void Add(Obj s)
    {
        if(s.objSaver.canSleep.can)
        {
            BedSit.sit += s.objSaver.canSleep.count;
        }
        if (s.objSaver.canSet.can)
        {
            SetSit.sit += s.objSaver.canSet.count;
        }
    }
    public void Remove(Obj s)
    {
        if (s.objSaver.canSleep.can)
        {
            BedSit.sit -= s.objSaver.canSleep.count;
        }
        if (s.objSaver.canSet.can)
        {
            SetSit.sit -= s.objSaver.canSet.count;
        }
    }
}
