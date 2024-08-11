using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResType : RawType
{
    public BuildingResType() : base()
    {

    }
}

[System.Serializable]
public class BuildingResSaver : RawSaver
{

}

[Map(),Class]
// ½¨Öþ×ÊÔ´
public class BuildingResObj : RawObj
{
    ///////////////////////////////////////

    public BuildingResObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}
