using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// ���� PAttribute �࣬�̳��� Attribute
public class MapAttribute : Attribute
{
    public Type type;
    public Type saver;
    // ���캯�������ڳ�ʼ�����Ե�ֵ
    public MapAttribute(Type type= null, Type saver = null)
    {
        this.type = type;
        this.saver = saver;
    }
}