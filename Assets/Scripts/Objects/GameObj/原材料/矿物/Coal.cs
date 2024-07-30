using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalType : KuangType
{
    public CoalType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class CoalSaver : KuangSaver
{

}

[Map()]
// ��
public class CoalObj : KuangObj
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

public class CoalMiningType : KuangType
{
    public CoalMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class CoalMiningSaver : KuangMiningSaver
{

}

[Map()]
// ����
public class CoalMiningObj : KuangMiningObj
{
    ///////////////////////////////////////
    public CoalMiningObj(CoalMiningSaver objAsset = null) : base(objAsset)
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
        return ObjEnum.CoalObjE;
    }
}