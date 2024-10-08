using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShuLinType : BuildingType
{
}

[System.Serializable]
public class ShuLinSaver : BuildingSaver
{

}

[Map(), Class]
// ���
public class ShuLinObj : BuildingObj
{
    ///////////////////////////////////////
    public ShuLinObj(ShuLinSaver objAsset = null) : base(objAsset)
    {
        Init();
        resource.Add(ObjEnum.TreeObjE,10000);
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public override void LatUpdate()
    {
        base.LatUpdate();
        str.Clear();
        str.AppendLine("Tree:"+resource.Get(ObjEnum.TreeObjE));
        str.AppendLine("Seed:"+resource.Get(ObjEnum.TreeSeedObjE));
        cardInf.description=str.ToString();
        cardInf.cardControl.UpdateInf();
    }
}