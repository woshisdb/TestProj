using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// ���� PAttribute �࣬�̳��� Attribute
public class MapAttribute : Attribute
{
    public Type type;
    // ���캯�������ڳ�ʼ�����Ե�ֵ
    public MapAttribute(Type type=null)
    {
        this.type = type;
    }
}