using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 食物
/// </summary>
public class FoodType : RawType
{
    public FoodType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class FoodSaver : ObjSaver
{

}
/// <summary>
/// 1点能量代表坚持一个回合
/// </summary>
[Map()]
public class FoodObj : RawObj
{
    public int energy;//食物所提供的能量
    public FoodObj(ObjSaver objAsset = null,int energy=1):base(objAsset)
    {

    }
    public FoodObj(ObjSaver objAsset = null) : base(objAsset)
    {
        energy = 1;
    }
}
