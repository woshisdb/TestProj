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
    [SerializeField]
    public int size=1;
    public CardInf cardInf;
    public ObjInf canSleep;//����˯��
    public ObjInf canSet;//����������
    public ObjInf canReg;//����ע��Э��
    public ObjInf canJoin;//����ǩ��Э��
    public ObjInf canKitch;//�����������ʳ��
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
    public ObjAsset()
    {
    }
}