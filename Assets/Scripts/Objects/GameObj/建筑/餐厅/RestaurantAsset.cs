using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ²ÍÌü
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
// ²ÍÌü
public class RestaurantObj : BuildingObj
{
    public RestaurantObj(BuildingSaver objAsset=null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts= base.InitActivities();
        return acts;
    }
    public RestaurantSaver GetSaver()
    {
        return (RestaurantSaver)objSaver;
    }
}