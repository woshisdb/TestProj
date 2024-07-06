using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class RestaurantType : BuildingType
{
    public RestaurantType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class RestaurantSaver : BuildingSaver
{
    
}

[Map()]
// ����
public class RestaurantObj : BuildingObj<BuildingSaver>
{
    public RestaurantObj(BuildingSaver objAsset=null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts= base.InitActivities();
        acts.Add(new BuyThingAct());//������Ʒ
        return acts;
    }
}