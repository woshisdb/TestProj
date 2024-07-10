using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmType : BuildingType
{
    public FarmType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class FarmSaver : BuildingSaver
{

}

[Map()]
// 农场
public class FarmObj : BuildingObj
{
    /// <summary>
    /// 耕种工具,
    /// </summary>
    public Rate gengZhong;
    /// <summary>
    /// 收获工具,
    /// </summary>
    public Rate shouHuo;
    /// <summary>
    /// 栽种工具,
    /// </summary>
    public Rate zaiZhong;
    /// <summary>
    /// 切割工具
    /// </summary>
    public Rate qieGe;
    ///////////////////////////////////////

    public FarmObj(BuildingSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public FarmSaver GetSaver()
    {
        return (FarmSaver)objSaver;
    }
}
