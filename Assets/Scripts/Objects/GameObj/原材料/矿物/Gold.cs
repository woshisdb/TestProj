using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKuangType : KuangType
{
    public GoldKuangType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class GoldKuangSaver : KuangSaver
{

}

[Map()]
// 金子
public class GoldKuangObj : KuangObj
{
    ///////////////////////////////////////

    public GoldKuangObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}

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
// 金子
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
// 金矿
public class GoldMiningObj : KuangMiningObj
{
    ///////////////////////////////////////
    public GoldMiningObj(GoldMiningSaver objAsset = null) : base(objAsset)
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
    public override ObjEnum GetSource()
    {
        return ObjEnum.GoldKuangObjE;
    }
}