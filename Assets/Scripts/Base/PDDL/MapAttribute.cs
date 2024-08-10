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

public class Enum_PDDL<T> : PDDLClass
where T : Enum
{
    /// <summary>
    /// 枚举值
    /// </summary>
    public Func<T> enumVal;
    public override List<Func> GetFuncs()
    {
        return null;
    }

    public override List<Pop> GetFuncsVal()
    {
        return null;
    }

    public override List<PAction> GetPActions()
    {
        return null;
    }

    public override List<Predicate> GetPreds()
    {
        return null;
    }

    public override List<Pop> GetPredsVal()
    {
        return null;
    }

    public override PType GetPType()
    {
        return new PType(GetType().Name);
    }

    public override void SetObj(object obj)
    {
        this.enumVal= (Func<T>)obj;
    }
    public override List<PType> GetObjs()
    {
        var ps= new List<PType>();
        foreach(var x in Enum.GetNames(typeof(T)))
        {
            ps.Add(new PType(GetType().Name,x));
        }
        return ps;
    }

    public override PType GetObj()
    {
        return new PType(GetType().Name, enumVal().ToString());
    }
}

public class ObjEnumType:PType
{

}

[Class]
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
    TreeSeedObjE,
    TreeObjE,
    WoodObjE,
    BuildingResObjE,//建筑的资源
    KuangObjE,
    KuangMiningObjE,
    //..........................建筑
    BuildingObjE,
    RestaurantObjE,
    FarmObjE,
    //.........................小麦相关
    WheatObjE,
    WheatSeedObjE,
    WheatTreeObjE,
    PlaceObjE,
    WheatPlaceObjE,
    FullWheatPlaceObjE,
    WheatFlourObjE,
    //.............矿物
    GoldKuangObjE,
    GoldObjE,
    GoldMiningObjE,
    IronKuangObjE,
    IronObjE,
    IronMiningObjE,
    CoalObjE,
    CoalMiningObjE,
    TaotuObjE,
    TaotuMiningObjE,
    ShuLinObjE,
    GongchangObjE,
    //.............
    MoneyObjE,
    ToolObjE
}