using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public class Tool
{
    public static T DeepClone<T>(T obj)
    {
        // ���л�����
        var serializedData = SerializationUtility.SerializeValue(obj, DataFormat.Binary);

        // �����л�����
        T clonedObject = SerializationUtility.DeserializeValue<T>(serializedData, DataFormat.Binary);

        return clonedObject;
    }
    public static bool IsSameClass(object obj1, object obj2)
    {
        return obj1.GetType() == obj2.GetType();
    }
}
/// <summary>
/// Int����������
/// </summary>
public class Int
{
    public int val;
    public Int(int val=0)
    {
        this.val = val;
    }
    // ��ʽת������ int �� IntWrapper
    public static implicit operator Int(int intValue)
    {
        return new Int(intValue);
    }

    // ��ʽת������ IntWrapper �� int
    public static implicit operator int(Int intWrapper)
    {
        return intWrapper.val;
    }
}
