using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SeedType : RawType
{
    public SeedType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class SeedSaver : RawSaver
{

}
[Map()]
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
