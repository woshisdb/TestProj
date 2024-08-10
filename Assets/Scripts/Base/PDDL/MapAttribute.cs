using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// ���� PAttribute �࣬�̳��� Attribute
public class MapAttribute : Attribute
{
    public Type type;
    public string saver;
    // ���캯�������ڳ�ʼ�����Ե�ֵ
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
    /// ö��ֵ
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
    //...........................����
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
    BuildingResObjE,//��������Դ
    KuangObjE,
    KuangMiningObjE,
    //..........................����
    BuildingObjE,
    RestaurantObjE,
    FarmObjE,
    //.........................С�����
    WheatObjE,
    WheatSeedObjE,
    WheatTreeObjE,
    PlaceObjE,
    WheatPlaceObjE,
    FullWheatPlaceObjE,
    WheatFlourObjE,
    //.............����
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