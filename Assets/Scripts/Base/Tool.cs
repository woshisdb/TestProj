using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public class Tool
{
    public static T DeepClone<T>(T obj)
    {
        // 序列化对象
        var serializedData = SerializationUtility.SerializeValue(obj, DataFormat.Binary);

        // 反序列化对象
        T clonedObject = SerializationUtility.DeserializeValue<T>(serializedData, DataFormat.Binary);

        return clonedObject;
    }
    public static bool IsSameClass(object obj1, object obj2)
    {
        return obj1.GetType() == obj2.GetType();
    }
}
/// <summary>
/// Int的引用类型
/// </summary>
public class Int
{
    public int val;
    public Int(int val=0)
    {
        this.val = val;
    }
    // 隐式转换：从 int 到 IntWrapper
    public static implicit operator Int(int intValue)
    {
        return new Int(intValue);
    }

    // 隐式转换：从 IntWrapper 到 int
    public static implicit operator int(Int intWrapper)
    {
        return intWrapper.val;
    }
}
