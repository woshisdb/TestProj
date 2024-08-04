using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodType : RawType
{
    public WoodType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class WoodSaver : RawSaver
{

}
[Map()]
public class WoodObj : RawObj
{
    public WoodObj(WoodSaver objSaver = null) : base(objSaver)
    {

    }
    public WoodSaver GetSaver()
    {
        return (WoodSaver)objSaver;
    }
}
