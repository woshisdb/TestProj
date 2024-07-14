using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ð¡Âó
/// </summary>
public class WheatFlourType : RawType
{
    public WheatFlourType(string name = null) : base(name)
    {

    }
}

[System.Serializable]
public class WheatFlourSaver : RawSaver
{
}

[Map()]
public class WheatFlourObj : RawObj
{
    public WheatFlourObj(ObjAsset objAsset)
    {

    }
    public WheatFlourObj()
    {

    }
}
