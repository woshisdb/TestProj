using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaotuType : KuangType
{
    public TaotuType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class TaotuSaver : KuangSaver
{

}

[Map()]
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
    public TaotuMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class TaotuMiningSaver : KuangMiningSaver
{
   
}

[Map()]
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