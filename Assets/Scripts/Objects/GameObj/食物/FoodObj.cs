using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ʳ��
/// </summary>
public class FoodType : RawType
{
}
[System.Serializable]
public class FoodSaver : RawSaver
{
    /// <summary>
    /// ԭ����Ŀ
    /// </summary>
    public Resource rawObj;
    /// <summary>
    /// ʳ�����ṩ������
    /// </summary>
    public int energy;
    /// <summary>
    /// ���������ѵĹ�ʱ
    /// </summary>
    public int wastTime;
}
/// <summary>
/// 1������������һ���غ�
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
/// ���϶���
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
/// ʳ��Saver
/// </summary>
public class FoodHashInf:HashInf
{
    /// <summary>
    /// ��ζ[0.1]
    /// </summary>
    public float foodPoint;
    /// <summary>
    /// �߶���[0.1]
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
    /// �𻵵ĸ���
    /// </summary>
    public int brokenRate;
}
/// <summary>
/// 1������������һ���غ�
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