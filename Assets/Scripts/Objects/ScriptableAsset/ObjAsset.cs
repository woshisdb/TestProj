using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
[System.Serializable]
public class ObjInf
{
    public bool can;//是否可以
    public int count;//提供数目
    public ObjInf()
    {
        can = false;
        count = 0;
    }
}

[System.Serializable]
public class ObjSaver
{
    /// <summary>
    /// 是否可以交易
    /// </summary>
    public bool canSell;
    [EnumPaging]
    public SaveTye saveTye;
    /// <summary>
    /// 对象类型
    /// </summary>
    [SerializeField]
    public int size=1;
    //public CardInf cardInf;
    public string title;
    public string description;
    [OdinSerialize]
    public Dic<TransationEnum, ObjInf> transPairs=new Dic<TransationEnum, ObjInf>();
    ////////////////////////////////////////////////////////
    [OdinSerialize]
    public Dic<SitEnum,int> sits=new Dic<SitEnum, int>();
    public int SitVal(SitEnum sitEnum)
    {
        if(sits.ContainsKey(sitEnum))
            return sits[sitEnum];
        else
            return 0;
    }
    public int TransCount(TransationEnum transationEnum)
    {
        if (transPairs.ContainsKey(transationEnum))
            return transPairs[transationEnum].count;
        else
            return 0;
    }
    public bool TransCan(TransationEnum transationEnum)
    {
        if (transPairs.ContainsKey(transationEnum))
            return transPairs[transationEnum].can;
        else
            return false;
    }
    public ObjEnum GetEnum()
    {
        return Map.Instance.saver2Enum[GetType()];

    }
}
public class Enum<T> where T : System.Enum
{
    public T value;
    public Enum(T v)
    {
        value = v;
    }
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

    [NonSerialized,OdinSerialize]
    public ObjSaver objSaver;
    [NonSerialized, OdinSerialize]
    public AnimalSaver animalSaver;
    [NonSerialized, OdinSerialize]
    public PersonSaver personSaver;
    [NonSerialized, OdinSerialize]
    public BedSaver bedSaver;
    [NonSerialized, OdinSerialize]
    public DeskSaver deskSaver;
    [NonSerialized, OdinSerialize]
    public PathSaver pathSaver;
    [NonSerialized, OdinSerialize]
    public RawSaver rawSaver;
    [NonSerialized, OdinSerialize]
    public FoodSaver foodSaver;
    [NonSerialized, OdinSerialize]
    public BuildingSaver buildingSaver;
    [NonSerialized, OdinSerialize]
    public RestaurantSaver restaurantSaver;
    [NonSerialized, OdinSerialize]
    public ZaozhuanSitSaver zaozhuanSitSaver;
    [NonSerialized, OdinSerialize]
    public SeedSaver seedSaver;
    [NonSerialized, OdinSerialize]
    public TreeSeedSaver treeSeedSaver;
    [NonSerialized, OdinSerialize]
    public TreeSaver treeSaver;
    [NonSerialized, OdinSerialize]
    public WoodSaver woodSaver;
    //...........................小麦相关
    [NonSerialized, OdinSerialize]
    public WheatSaver wheatSaver;
    [NonSerialized, OdinSerialize]
    public WheatTreeSaver wheatTreeSaver;
    [NonSerialized, OdinSerialize, EnumPaging]
    public WheatSeedSaver wheatSeedSaver;
    [NonSerialized, OdinSerialize, EnumPaging]
    public WheatPlaceSaver wheatPlaceSaver;
    [NonSerialized, OdinSerialize, EnumPaging]
    public FullWheatPlaceSaver fullWheatPlaceSaver;
    public PlaceSaver placeSaver;
    [NonSerialized, OdinSerialize]
    public WheatFlourSaver wheatFlourSaver;
    //...........................
    [NonSerialized, OdinSerialize]
    public KuangSaver kuangSaver;
    [NonSerialized, OdinSerialize]
    public KuangMiningSaver kuangMiningSaver;
    [NonSerialized, OdinSerialize]
    public GoldKuangSaver goldKuangSaver;
    [NonSerialized, OdinSerialize]
    public GoldSaver goldSaver;
    [NonSerialized, OdinSerialize]
    public GoldMiningSaver goldMiningSaver;
    [NonSerialized, OdinSerialize]
    public IronKuangSaver ironKuangSaver;
    [NonSerialized, OdinSerialize]
    public IronSaver ironSaver;
    [NonSerialized, OdinSerialize]
    public IronMiningSaver ironMiningSaver;
    [NonSerialized, OdinSerialize]
    public CoalSaver coalSaver;
    [NonSerialized, OdinSerialize]
    public CoalMiningSaver coalMiningSaver;
    [NonSerialized, OdinSerialize]
    public TaotuSaver taotuSaver;
    [NonSerialized, OdinSerialize]
    public TaotuMiningSaver taotuMiningSaver;
    [NonSerialized, OdinSerialize]
    public ShuLinSaver shuLinSaver;
    [NonSerialized, OdinSerialize]
    public GongchangSaver gongchangSaver;
    //....................
    [NonSerialized, OdinSerialize]
    public FarmSaver farmSaver;
    [NonSerialized, OdinSerialize]
    public MoneySaver moneySaver;
    [NonSerialized, OdinSerialize]
    public ToolSaver toolSaver;
    [NonSerialized, OdinSerialize]
    public BuildingResSaver buildingResSaver;
    public ObjAsset()
    {
    }
}