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
    [SerializeField]
    public int size=1;
    public CardInf cardInf;
    public ObjInf canSleep;//可以睡觉
    public ObjInf canSet;//可以坐下来
    public ObjInf canReg;//可以注册协议
    public ObjInf canJoin;//可以签署协议
    public ObjInf canKitch;//可以用于烹饪食物
}

/// <summary>
/// 对象资源
/// </summary>
[System.Serializable,CreateAssetMenu(fileName = "NewObjAsset", menuName = "ScriptableObjects/ObjAsset")]
public class ObjAsset : SerializedScriptableObject
{
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
    public ObjAsset()
    {
    }
}