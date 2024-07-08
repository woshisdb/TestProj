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
    public ObjInf canSleep;//����˯��
    public ObjInf canSet;//����������
    //public ObjInf canReg;//����ע��Э��
    //public ObjInf canJoin;//����ǩ��Э��
    public ObjInf canCook;//�����������ʳ��
}

public enum ObjEnum
{
    /*****************��������******************/
    Restaurant,
    /*****************ʳ�����******************/
    /*****************ԭ�϶���******************/
    /*****************�Ҿ߶���******************/
}

/// <summary>
/// ������Դ
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
    /// <summary>
    /// һϵ�е�ʳ��
    /// </summary>
    [SerializeField]
    public List<FoodSaver> foods;
    public ObjAsset()
    {
    }
}