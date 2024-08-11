using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaotuType : KuangType
{
}
[System.Serializable]
public class TaotuSaver : KuangSaver
{

}

[Map(),Class]
// ½ð×Ó
public class TaotuObj : KuangObj
{
    ///////////////////////////////////////

    public TaotuObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}

public class TaotuMiningType : KuangMiningType
{
}
[System.Serializable]
public class TaotuMiningSaver : KuangMiningSaver
{
   
}

[Map(),Class]
// ½ð¿ó
public class TaotuMiningObj : KuangMiningObj
{
    ///////////////////////////////////////
    public TaotuMiningObj(TaotuMiningSaver objAsset = null) : base(objAsset)
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
        return ObjEnum.TaotuObjE;
    }
}