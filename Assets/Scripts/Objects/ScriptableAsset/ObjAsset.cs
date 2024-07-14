using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjInf
{
    public bool can;//�Ƿ����
    public int count;//�ṩ��Ŀ
}

[System.Serializable]
public class ObjSaver
{
    /// <summary>
    /// ��������
    /// </summary>
    public ObjEnum objEnum;
    [SerializeField]
    public int size=1;
    //public CardInf cardInf;
    public string title;
    public string description;
    public ObjInf qieGe;//����˯��
    public ObjInf shouHuo;//����������
    public ObjInf canCook;//�����������ʳ��
    public ObjInf gengZhong;//�����������ʳ��
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


    /// <summary>
    /// һϵ�е�ʳ��
    /// </summary>
    [SerializeField]
    public List<FoodSaver> foods;
    public ObjAsset()
    {
    }
}