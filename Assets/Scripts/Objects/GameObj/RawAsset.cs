using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawType : PType
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
    public RawObj(RawSaver objAsset = null):base(objAsset)
    {
        //Debug.Log(objAsset);
    }
    public RawSaver GetSaver()
    {
        return (RawSaver)objSaver;
    }
}



[System.Serializable]
public class RawSaver : ObjSaver
{
}
