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
/// 

public interface IHashInf
{
    public HashInf GetInf();
}


public class HashInf:IEqualityComparer<HashInf>
{
    int id;

    public bool Equals(HashInf x, HashInf y)
    {
        return x.id == y.id;
    }

    public int GetHashCode(HashInf obj)
    {
        return obj.id.GetHashCode();
    }
}

/// <summary>
/// 集合对象
/// </summary>
public abstract class HashSetObj<T>: RawObj, IEqualityComparer<T>, IHashInf
where T:IHashInf
{
    public bool Equals(T x, T y)
    {
        return x.GetInf().Equals(y.GetInf());
    }

    public int GetHashCode(FoodObj obj)
    {
        return obj.GetInf().GetHashCode();
    }

    public int GetHashCode(T obj)
    {
        throw new System.NotImplementedException();
    }

    public abstract HashInf GetInf();

    public FoodSaver GetSaver()
    {
        return (FoodSaver)objSaver;
    }
}
/// <summary>
/// 食物Saver
/// </summary>
public class FoodHashInf:HashInf
{
    /// <summary>
    /// 口味[0.1]
    /// </summary>
    public float foodPoint;
    /// <summary>
    /// 高端吗[0.1]
    /// </summary>
    public float level;
}

[Map(), Class]
public class FoodObj : HashSetObj<FoodObj>
{
    public override HashInf GetInf()
    {
        return new FoodHashInf();
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