using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaozhuanSitType: BuildingType
{
    public ZaozhuanSitType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class ZaozhuanSitSaver : BuildingSaver
{
    
}

[Map()]
// Å©³¡
public class ZaozhuanSitObj : BuildingObj
{
    ///////////////////////////////////////

    public ZaozhuanSitObj(BuildingSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}
