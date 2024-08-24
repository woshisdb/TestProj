using System.Collections;
using System.Collections.Generic;
using QFramework;
//using Unity.VisualScripting;
using UnityEngine;

public class PersonObjControler : MonoBehaviour,IController
{
    public PersonObj PersonObj;

    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
}
