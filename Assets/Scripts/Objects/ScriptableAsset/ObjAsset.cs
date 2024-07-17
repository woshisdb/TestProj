using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjInf
{
    public bool can;//是否可以
    public int count;//提供数目
}

[System.Serializable]
public class ObjSaver
{
    [EnumPaging]
    public SaveTye saveTye;
    /// <summary>
    /// 对象类型
    /// </summary>
    [EnumPaging]
    public ObjEnum objEnum;
    [SerializeField]
    public int size=1;
    //public CardInf cardInf;
    public string title;
    public string description;
    public ObjInf qieGe;//可以睡觉
    public ObjInf shouHuo;//可以坐下来
    public ObjInf canCook;//可以用于烹饪食物
    public ObjInf gengZhong;//可以用于烹饪食物
    public ObjInf zaiZhong;//可以用于烹饪食物
    ////////////////////////////////////////////////////////
    public int sleep;//睡觉
    public int set;//坐

}

public enum ObjEnum
{
    PersonE,
    BedObjE,
    DeskObjE,
    AnimalObjE,
    ObjE,
    PathObjE,
    RawObjE,
    FoodObjE,
    BuildingObjE,
    RestaurantObjE,
    SeedObjE,
    TreeObjE,
    WheatObjE,
    WheatSeedObjE,
    WheatTreeObjE,
    PlaceObjE,
    WheatPlaceObjE,
    FullWheatPlaceObjE,
    MoneyObjE,
    WheatFlourObjE,
    FarmObjE,
    ToolObjE
}

public enum SaveTye
{
    /// <summary>
    /// 按照时间轴保存
    /// </summary>
    day,
    month,
    year,
    /// <summary>
    /// 只需按照集合保存
    /// </summary>
    set,
    /// <summary>
    /// 保存唯一一个
    /// </summary>
    single,
}

/// <summary>
/// 对象资源
/// </summary>
[System.Serializable,CreateAssetMenu(fileName = "NewObjAsset", menuName = "ScriptableObjects/ObjAsset")]
public class ObjAsset : SerializedScriptableObject
{
    [SerializeField]
    public WorldMap map;
    /// <summary>
    /// 节点图
    /// </summary>
    [SerializeField]
    public NodeGraph nodeGraph;
    /// <summary>
    /// 默认构造Saver
    /// </summary>
    [SerializeField]
    public PersonSaver personSaver;
    [SerializeField]
    public AnimalSaver animalSaver;
    [SerializeField]
    public BedSaver bedSaver;
    [SerializeField]
    public DeskSaver deskSaver;
    [SerializeField]
    public ObjSaver objSaver;
    [SerializeField]
    public PathSaver pathSaver;
    [SerializeField]
    public RawSaver rawSaver;
    [SerializeField]
    public FoodSaver foodSaver;
    [SerializeField]
    public BuildingSaver buildingSaver;
    [SerializeField]
    public RestaurantSaver restaurantSaver;
    [SerializeField]
    public SeedSaver seedSaver;
    [SerializeField]
    public TreeSaver treeSaver;
    [SerializeField]
    public WheatSaver wheatSaver;
    [SerializeField]
    public WheatTreeSaver wheatTreeSaver;
    [SerializeField]
    public WheatSeedSaver wheatSeedSaver;
    [SerializeField]
    public WheatPlaceSaver wheatPlaceSaver;
    [SerializeField]
    public FullWheatPlaceSaver fullWheatPlaceSaver;
    [SerializeField]
    public PlaceSaver placeSaver;
    [SerializeField]
    public WheatFlourSaver wheatFlourSaver;
    [SerializeField]
    public FarmSaver farmSaver;
    [SerializeField]
    public MoneySaver moneySaver;
    [SerializeField]
    public ToolSaver toolSaver;


    /// <summary>
    /// 一系列的食物
    /// </summary>
    [SerializeField]
    public List<FoodSaver> foods;
    public ObjAsset()
    {
    }
}