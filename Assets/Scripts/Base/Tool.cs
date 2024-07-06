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
