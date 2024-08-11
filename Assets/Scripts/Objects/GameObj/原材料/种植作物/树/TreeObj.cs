using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeType : RawType
{
}
[System.Serializable]
public class TreeSaver : RawSaver
{

}
[Map(),Class]
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
