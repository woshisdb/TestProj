using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ð¡Âó
/// </summary>
public class WheatType : RawType
{
}
public class WheatSaver:RawSaver
{

}
[Map(),Class]
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
