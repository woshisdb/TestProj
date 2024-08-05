using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����

[System.Serializable]
public class RestaurantSaver : BuildingSaver
{
    
}

[Map()]
// ����
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