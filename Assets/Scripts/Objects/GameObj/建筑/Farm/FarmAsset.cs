using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FarmSaver : BuildingSaver
{

}

[Map()]
// ũ��
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
        return acts;
    }
}
