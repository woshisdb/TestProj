using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalType : RawType
{
    public CoalType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class CoalSaver : RawSaver
{

}

[Map()]
// Ìú
public class CoalObj : RawObj
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

public class CoalMiningType : RawType
{
    public CoalMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class CoalMiningSaver : RawSaver
{

}

[Map()]
// Ìú¿ó
public class CoalMiningObj : RawObj
{
    ///////////////////////////////////////

    public CoalMiningObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}