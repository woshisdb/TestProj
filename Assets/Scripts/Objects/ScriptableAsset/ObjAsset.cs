using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
[System.Serializable]
public class ObjInf
{
    public bool can;//�Ƿ����
    public int count;//�ṩ��Ŀ
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
    /// �Ƿ���Խ���
    /// </summary>
    public bool canSell;
    [EnumPaging]
    public SaveTye saveTye;
    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField]
    public int size=1;
    //public CardInf cardInf;
    public string title;
    public string description;
    [OdinSerialize]
    public Dictionary<TransationEnum, ObjInf> transPairs=new Dictionary<TransationEnum, ObjInf>();
    ////////////////////////////////////////////////////////
    [OdinSerialize]
    public Dictionary<SitEnum,int> sits=new Dictionary<SitEnum, int>();
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
    /// ����ʱ���ᱣ��
    /// </summary>
    day,
    month,
    year,
    /// <summary>
    /// ֻ�谴�ռ��ϱ���
    /// </summary>
    set,
    /// <summary>
    /// ����Ψһһ��
    /// </summary>
    single,
}

/// <summary>
/// ������Դ
/// </summary>
[System.Serializable,CreateAssetMenu(fileName = "NewObjAsset", menuName = "ScriptableObjects/ObjAsset")]
public class ObjAsset : SerializedScriptableObject
{
    [SerializeField]
    public WorldMap map;
    /// <summary>
    /// �ڵ�ͼ
    /// </summary>
    [SerializeField]
    public NodeGraph nodeGraph;
    /// <summary>
    /// Ĭ�Ϲ���Saver
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
    public MiningSitSaver miningSitSaver;
    [NonSerialized, OdinSerialize]
    public SeedSaver seedSaver;
    [NonSerialized, OdinSerialize]
    public TreeSaver treeSaver;
    //...........................С�����
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
    public GoldSaver goldSaver;
    [NonSerialized, OdinSerialize]
    public GoldMiningSaver goldMiningSaver;
    [NonSerialized, OdinSerialize]
    public IronSaver ironSaver;
    [NonSerialized, OdinSerialize]
    public IronMiningSaver ironMiningSaver;
    [NonSerialized, OdinSerialize]
    public CoalSaver coalSaver;
    [NonSerialized, OdinSerialize]
    public CoalMiningSaver coalMiningSaver;
    //....................
    [NonSerialized, OdinSerialize]
    public FarmSaver farmSaver;
    [NonSerialized, OdinSerialize]
    public MoneySaver moneySaver;
    [NonSerialized, OdinSerialize]
    public ToolSaver toolSaver;


    /// <summary>
    /// һϵ�е�ʳ��
    /// </summary>
    [NonSerialized, OdinSerialize]
    public List<FoodSaver> foods;
    public ObjAsset()
    {
    }
}