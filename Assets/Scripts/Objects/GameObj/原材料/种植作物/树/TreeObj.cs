using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeType : RawType
{
    public TreeType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class TreeSaver : RawSaver
{

}
[Map()]
public class TreeObj : RawObj
{
    public TreeObj(TreeSaver objSaver = null) : base(objSaver)
    {

    }
    public TreeSaver GetSaver()
    {
        return (TreeSaver)objSaver;
    }
}
