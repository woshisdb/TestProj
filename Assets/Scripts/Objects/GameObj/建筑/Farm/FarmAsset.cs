using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmType : BuildingType
{
    public FarmType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class FarmSaver : BuildingSaver
{

}

[Map()]
// Å©³¡
public class FarmObj : BuildingObj
{
    ///////////////////////////////////////

    public FarmObj(BuildingSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        acts.Add(new SelPipLineAct());
        return acts;
    }
    public FarmSaver GetSaver()
    {
        return (FarmSaver)objSaver;
    }
}
