using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalType: ObjType
{
    public AnimalType():base()
    {

    }
}
[System.Serializable]
public class AnimalSaver : ObjSaver
{
    [SerializeField]
    public bool sex;
    [SerializeField]
    public int age;
    [SerializeField]
    public bool hasSelect;
    [SerializeField]
    public bool isUseObj;
    [SerializeField]
    public PersonSVal sleepState;
}
[Map]
public class AnimalObj : Obj
{
    public bool sex;//�Ա�
    public int age;//����
    public bool hasSelect;//�Ƿ���ѡ��
    /******************************************************/
    public PersonState sleepState;//˯�ߵ�״̬
    public PersonState foodState;//ʳ��ĵ�״̬
    public AnimalObj(ObjSaver objAsset=null):base(objAsset)
    {
        Init();
    }
    public void Init()
    {
        AnimalSaver sv = (AnimalSaver)objSaver;
        sex = sv.sex;
        age = sv.age;
        hasSelect =false;
        sleepState = new PersonState((AnimalObj)this,sv.sleepState);
    }
    public AnimalSaver GetSaver()
    {
        return (AnimalSaver)objSaver;
    }
}
