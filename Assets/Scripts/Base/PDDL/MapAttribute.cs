using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// 定义 PAttribute 类，继承自 Attribute
public class MapAttribute : Attribute
{
    public Type type;
    // 构造函数，用于初始化属性的值
    public MapAttribute(Type type=null)
    {
        this.type = type;
    }
}