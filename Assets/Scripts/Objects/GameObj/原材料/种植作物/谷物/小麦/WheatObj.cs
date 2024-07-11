using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ð¡Âó
/// </summary>
public class WheatType : RawType
{
    public WheatType(string name = null) : base(name)
    {

    }
}
public class WheatSaver:RawSaver
{

}
[Map()]
public class WheatObj : RawObj
{
    public WheatObj(WheatSaver objSaver=null):base(objSaver)
    {

    }
    public WheatSaver GetSaver()
    {
        return (WheatSaver)objSaver;
    }
}
