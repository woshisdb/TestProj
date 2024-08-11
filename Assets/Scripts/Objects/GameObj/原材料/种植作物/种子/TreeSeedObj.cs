using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TreeSeedType : SeedType
{
}
[System.Serializable]
public class TreeSeedSaver : SeedSaver
{
    
}
[Map(),Class]
public class TreeSeedObj : SeedObj
{
    public TreeSeedObj(TreeSeedSaver objSaver = null) : base(objSaver)
    {

    }
    public TreeSeedSaver GetSaver()
    {
        return (TreeSeedSaver)objSaver;
    }
}
