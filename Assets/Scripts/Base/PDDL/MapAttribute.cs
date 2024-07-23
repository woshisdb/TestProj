using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// 定义 PAttribute 类，继承自 Attribute
public class MapAttribute : Attribute
{
    public Type type;
    public string saver;
    // 构造函数，用于初始化属性的值
    public MapAttribute(Type type= null, string saver = null)
    {
        this.type = type;
        this.saver = saver;
    }
}
public enum ObjEnum
{
    //...........................基类
    PersonE,
    BedObjE,
    DeskObjE,
    AnimalObjE,
    ObjE,
    PathObjE,
    RawObjE,
    FoodObjE, 
    SeedObjE,
    TreeObjE,
    BuildingResObjE,//建筑的资源
    KuangObjE,
    KuangMiningObjE,
    //..........................建筑
    BuildingObjE,
    RestaurantObjE,
    FarmObjE,
    MiningSitObjE,
    //.........................小麦相关
    WheatObjE,
    WheatSeedObjE,
    WheatTreeObjE,
    PlaceObjE,
    WheatPlaceObjE,
    FullWheatPlaceObjE,
    WheatFlourObjE,
    //.............矿物
    GoldObjE,
    GoldMiningObjE,
    IronObjE,
    IronMiningObjE,
    CoalObjE,
    CoalMiningObjE,
    //.............
    MoneyObjE,
    ToolObjE
}