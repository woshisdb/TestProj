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