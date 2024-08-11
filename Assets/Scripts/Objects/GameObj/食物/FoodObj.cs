using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 食物
/// </summary>
public class FoodType : RawType
{
}
[System.Serializable]
public class FoodSaver : RawSaver
{
    /// <summary>
    /// 原料数目
    /// </summary>
    public Resource rawObj;
    /// <summary>
    /// 食物所提供的能量
    /// </summary>  
    public int energy;
    /// <summary>
    /// 生产所花费的工时
    /// </summary>
    public int wastTime;
}
/// <summary>
/// 1点能量代表坚持一个回合
/// </summary>
[Map(),Class]
public class FoodObj : RawObj
{

    public FoodObj(FoodSaver objAsset = null) : base(objAsset)
    {
    }
    public FoodSaver GetSaver()
    {
        return (FoodSaver)objSaver;
    }
}

public class MoneyType : RawType
{
    public MoneyType() : base()
    {

    }
}
[System.Serializable]
public class MoneySaver : RawSaver
{
}
[Map(), Class]
public class MoneyObj : RawObj
{
    public MoneyObj(MoneySaver objAsset = null) : base(objAsset)
    {
    }
    public MoneySaver GetSaver()
    {
        return (MoneySaver)objSaver;
    }
}
public class ToolType : ObjType
{
}

[System.Serializable]
public class ToolSaver:ObjSaver
{
    /// <summary>
    /// 损坏的概率
    /// </summary>
    public int brokenRate;
}
/// <summary>
/// 1点能量代表坚持一个回合
/// </summary>
[Map(), Class]
public class ToolObj : Obj
{
    public ToolObj(ToolSaver objAsset = null) : base(objAsset)
    {
    }
    public ToolSaver GetSaver()
    {
        return (ToolSaver)objSaver;
    }
}