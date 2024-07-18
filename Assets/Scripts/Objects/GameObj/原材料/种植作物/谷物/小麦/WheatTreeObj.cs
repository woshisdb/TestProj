using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ð¡Âó
/// </summary>
public class WheatTreeType : TreeType
{
    public WheatTreeType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class WheatTreeSaver : TreeSaver
{

}
[Map()]
public class WheatTreeObj : TreeObj
{
    public WheatTreeObj(WheatTreeSaver objSaver = null) : base(objSaver)
    {

    }
    public WheatTreeSaver GetSaver()
    {
        return (WheatTreeSaver)objSaver;
    }
}
