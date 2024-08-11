using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RestaurantType : BuildingType
{
}
// ����

[System.Serializable]
public class RestaurantSaver : BuildingSaver
{
    
}

[Map(),Class]
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