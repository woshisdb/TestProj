using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ð¡Âó
/// </summary>
public class WheatSeedType : SeedType
{
    public WheatSeedType(string name = null) : base(name)
    {

    }
}
public class WheatSeedSaver : SeedSaver
{

}
[Map()]
public class WheatSeedObj : SeedObj
{
    public WheatSeedObj(WheatSeedSaver objSaver = null) : base(objSaver)
    {

    }
    public WheatSeedSaver GetSaver()
    {
        return (WheatSeedSaver)objSaver;
    }
}

