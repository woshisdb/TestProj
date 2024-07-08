using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawType : ObjType
{
    public RawType(string name = null) : base(name)
    {

    }
}

[Map()]
/// <summary>
/// »ù´¡×ÊÔ´
/// </summary>
public class RawObj : Obj
{
    public RawObj(ObjSaver objAsset = null):base(objAsset)
    {

    }
    public RawObj()
    {

    }
    public RawSaver GetSaver()
    {
        return (RawSaver)objSaver;
    }
}



[System.Serializable]
public class RawSaver : ObjSaver
{
    public RawSaver(ObjAsset objAsset)
    {

    }
    public RawSaver()
    {
    }
}
