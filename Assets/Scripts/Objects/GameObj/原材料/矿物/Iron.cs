using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronType : RawType
{
    public IronType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class IronSaver : RawSaver
{

}

[Map()]
// ��
public class IronObj : RawObj
{
    ///////////////////////////////////////

    public IronObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}

public class IronMiningType : RawType
{
    public IronMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class IronMiningSaver : RawSaver
{

}

[Map()]
// ����
public class IronMiningObj : RawObj
{
    ///////////////////////////////////////

    public IronMiningObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}