using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatPlaceType : PlaceType
{
    public WheatPlaceType(string name = null) : base(name)
    {

    }
}
public class WheatPlaceSaver : PlaceSaver
{

}
[Map()]
public class WheatPlaceObj : PlaceObj
{
    public WheatPlaceObj(WheatPlaceSaver objSaver = null) : base(objSaver)
    {

    }
}


public class FullWheatPlaceType : PlaceType
{
    public FullWheatPlaceType(string name = null) : base(name)
    {

    }
}
public class FullWheatPlaceSaver : PlaceSaver
{

}
[Map()]
public class FullWheatPlaceObj : PlaceObj
{
    public FullWheatPlaceObj(FullWheatPlaceSaver objSaver = null) : base(objSaver)
    {

    }
}

public class PlaceType : ObjType
{
    public PlaceType(string name = null) : base(name)
    {

    }
}
[SerializeField]
public class PlaceSaver : ObjSaver
{
    public int xl;
    public PlaceSaver()
    {
        
    }
}
[Map()]
public class PlaceObj : Obj
{
    public PlaceObj(PlaceSaver objSaver = null) : base(objSaver)
    {

    }
}