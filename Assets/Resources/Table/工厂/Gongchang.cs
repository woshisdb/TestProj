using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GongchangType : BuildingType
{
    public GongchangType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class GongchangSaver : BuildingSaver
{

}

[Map()]
// ½ð¿ó
public class GongchangObj : BuildingObj
{
    ///////////////////////////////////////
    public GongchangObj(GongchangSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public override void LatUpdate()
    {
        base.LatUpdate();
        //str.Clear();
        //str.AppendLine("Tree:" + resource.Get(ObjEnum.TreeObjE));
        //str.AppendLine("Seed:" + resource.Get(ObjEnum.TreeSeedObjE));
        //cardInf.description = str.ToString();
        //cardInf.cardControl.UpdateInf();
    }
}