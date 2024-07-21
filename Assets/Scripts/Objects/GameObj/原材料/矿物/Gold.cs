using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldType : RawType
{
    public GoldType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class GoldSaver : RawSaver
{

}

[Map()]
// ½ð×Ó
public class GoldObj : RawObj
{
    ///////////////////////////////////////

    public GoldObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}

public class GoldMiningType : RawType
{
    public GoldMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class GoldMiningSaver : RawSaver
{
   
}

[Map()]
// ½ð¿ó
public class GoldMiningObj : RawObj
{
    ///////////////////////////////////////

    public GoldMiningObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}