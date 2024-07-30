using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KuangType : RawType
{
    public KuangType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class KuangSaver : RawSaver
{

}

[Map()]
// 铁
public class KuangObj : RawObj
{
    ///////////////////////////////////////

    public KuangObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}

public class KuangMiningType : RawType
{
    public KuangMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class KuangMiningSaver : BuildingSaver
{

}

[Map()]
// 铁矿
public class KuangMiningObj : BuildingObj
{
    ///////////////////////////////////////
    /// <summary>
    /// 初始资源的数目
    /// </summary>
    public int starSource;
    public virtual ObjEnum GetObj()
    {
        return ObjEnum.KuangObjE;
    }
    public KuangMiningObj(KuangMiningSaver objAsset = null) : base(objAsset)
    {
        Init();
        starSource = 1000;
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public override void LatUpdate()
    {
        str.Clear();
        //str.AppendLine(":");
        //str.AppendLine(source.ToString());
        ////后续更新
        //this.cardInf.description =
        str.AppendLine(GetObj().ToString());
        str.AppendLine(resource.resources[GetObj()] +":" +starSource);
        cardInf.description = str.ToString();
        if (cardInf.cardControl)
            cardInf.cardControl.UpdateInf();
    }
    public int GetResCount()
    {
        return resource.resources[GetObj()].remain;
    }
    /// <summary>
    /// 挖掘资源
    /// </summary>
    /// <param name=""></param>
    public KeyValuePair<ObjEnum, int> GetRes(int res)
    {
        if(GetResCount() >= starSource*0.7)
        {
            int sum = Mathf.Min(res, GetResCount());
            resource.Remove(GetObj(),sum);
            return new KeyValuePair<ObjEnum, int>(GetObj(),sum);
        }
        else if(GetResCount() >= 0.3*starSource)
        {
            int sum = Mathf.Min(res*7/10, GetResCount());
            resource.Remove(GetObj(), sum);
            return new KeyValuePair<ObjEnum, int>(GetObj(), sum);
        }
        else
        {
            int sum = Mathf.Min(res * 3 / 10, GetResCount());
            resource.Remove(GetObj(), sum);
            return new KeyValuePair<ObjEnum, int>(GetObj(), sum);
        }
    }
}


public class IronType : KuangType
{
    public IronType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class IronSaver : KuangSaver
{

}

[Map()]
// 铁
public class IronObj : KuangObj
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

public class IronMiningType : KuangMiningType
{
    public IronMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class IronMiningSaver : KuangMiningSaver
{
    
}

[Map()]
// 铁矿
public class IronMiningObj : KuangMiningObj
{
    public IronMiningObj(IronMiningSaver objAsset = null) : base(objAsset)
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
        return ObjEnum.IronObjE;
    }
}