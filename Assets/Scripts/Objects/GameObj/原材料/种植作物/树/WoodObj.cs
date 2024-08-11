using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodType : RawType
{
}
[System.Serializable]
public class WoodSaver : RawSaver
{

}
[Map(),Class]
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
