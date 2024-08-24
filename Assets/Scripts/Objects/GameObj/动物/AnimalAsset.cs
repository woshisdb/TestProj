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
}
[Map, Class]
public class AnimalObj : Obj
{
    public bool sex;//ÐÔ±ð
    public int age;//ÄêÁä
    public AnimalObj(ObjSaver objAsset=null):base(objAsset)
    {
        Init();
    }
    public void Init()
    {
        AnimalSaver sv = (AnimalSaver)objSaver;
    }
    public AnimalSaver GetSaver()
    {
        return (AnimalSaver)objSaver;
    }
}
