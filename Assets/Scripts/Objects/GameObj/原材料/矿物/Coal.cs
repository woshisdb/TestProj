using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalType : KuangType
{
}
[System.Serializable]
public class CoalSaver : KuangSaver
{

}

[Map(),Class]
// Ìú
public class CoalObj : KuangObj
{
    ///////////////////////////////////////

    public CoalObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}

public class CoalMiningType : KuangType
{
}
[System.Serializable]
public class CoalMiningSaver : KuangMiningSaver
{

}

[Map(),Class]
// Ìú¿ó
public class CoalMiningObj : KuangMiningObj
{
    ///////////////////////////////////////
    public CoalMiningObj(CoalMiningSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public override ObjEnum GetObj()
    {
        return ObjEnum.CoalObjE;
    }
}