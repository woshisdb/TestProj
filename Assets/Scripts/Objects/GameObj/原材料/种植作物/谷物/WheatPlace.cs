using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatPlaceType : PlaceType
{
}
public class WheatPlaceSaver : PlaceSaver
{

}
[Map(),Class]
public class WheatPlaceObj : PlaceObj
{
    public WheatPlaceObj(WheatPlaceSaver objSaver = null) : base(objSaver)
    {

    }
}
public class FullWheatPlaceType : PlaceType
{
}

public class FullWheatPlaceSaver : PlaceSaver
{

}
[Map(),Class]
public class FullWheatPlaceObj : PlaceObj
{
    public FullWheatPlaceObj(FullWheatPlaceSaver objSaver = null) : base(objSaver)
    {

    }
}

public class PlaceType : ObjType
{
    public PlaceType() : base()
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
[Map(),Class]
public class PlaceObj : Obj
{
    public PlaceObj(PlaceSaver objSaver = null) : base(objSaver)
    {

    }
}