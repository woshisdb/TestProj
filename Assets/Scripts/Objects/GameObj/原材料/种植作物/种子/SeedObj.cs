using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SeedType : RawType
{
}
[System.Serializable]
public class SeedSaver : RawSaver
{

}
[Map(),Class]
public class SeedObj : RawObj
{
    public SeedObj(SeedSaver objSaver = null) : base(objSaver)
    {

    }
    public SeedSaver GetSaver()
    {
        return (SeedSaver)objSaver;
    }
}
