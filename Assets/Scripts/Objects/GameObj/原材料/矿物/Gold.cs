using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldType : KuangType
{
    public GoldType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class GoldSaver : KuangSaver
{

}

[Map()]
// ½ð×Ó
public class GoldObj : KuangObj
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

public class GoldMiningType : KuangMiningType
{
    public GoldMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class GoldMiningSaver : KuangMiningSaver
{
   
}

[Map()]
// ½ð¿ó
public class GoldMiningObj : KuangMiningObj
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
    public override ObjEnum GetObj()
    {
        return ObjEnum.GoldObjE;
    }
}