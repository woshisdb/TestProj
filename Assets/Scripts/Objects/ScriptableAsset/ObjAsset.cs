using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
    [EnumPaging]
    public SaveTye saveTye;
    /// <summary>
    /// ��������
    /// </summary>
    [EnumPaging]
    public ObjEnum objEnum;
    [SerializeField]
    public int size=1;
    //public CardInf cardInf;
    public string title;
    public string description;
    [SerializeField]
    public ObjInf qieGe;//����˯��
    [SerializeField]
    public ObjInf shouHuo;//����������
    [SerializeField]
    public ObjInf canCook;//�����������ʳ��
    [SerializeField]
    public ObjInf gengZhong;//�����������ʳ��
    [SerializeField]
    public ObjInf zaiZhong;//�����������ʳ��
    ////////////////////////////////////////////////////////
    public int sleep;//˯��
    public int set;//��

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
    [SerializeField, EnumPaging]
    public WheatSeedSaver wheatSeedSaver;
    [SerializeField, EnumPaging]
    public WheatPlaceSaver wheatPlaceSaver;
    [SerializeField, EnumPaging]
    public FullWheatPlaceSaver fullWheatPlaceSaver;
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
    /// һϵ�е�ʳ��
    /// </summary>
    [SerializeField]
    public List<FoodSaver> foods;
    public ObjAsset()
    {
    }
}