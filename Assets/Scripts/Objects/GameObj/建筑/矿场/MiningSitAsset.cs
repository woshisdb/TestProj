using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningSitType: BuildingType
{
    public MiningSitType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class MiningSitSaver : BuildingSaver
{

}

[Map()]
// Å©³¡
public class MiningSitObj : BuildingObj
{
    ///////////////////////////////////////

    public MiningSitObj(BuildingSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}
