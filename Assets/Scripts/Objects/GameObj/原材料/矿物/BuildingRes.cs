using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResType : RawType
{
    public BuildingResType(string name = null) : base(name)
    {

    }
}

[System.Serializable]
public class BuildingResSaver : RawSaver
{

}

[Map()]
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
